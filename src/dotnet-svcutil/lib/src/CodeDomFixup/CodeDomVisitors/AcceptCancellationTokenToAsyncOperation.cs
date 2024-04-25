// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Tools.ServiceModel.Svcutil
{
    using System;
    using Microsoft.CodeDom;

    internal class AcceptCancellationTokenToAsyncOperation : ClientClassVisitor
    {
        protected override void VisitClientClass(CodeTypeDeclaration type)
        {
            base.VisitClientClass(type);
            
            for(int idx = type.Members.Count - 1; idx >= 0; idx--)
            {
                var method = type.Members[idx] as CodeMemberMethod;

                if (CodeDomHelpers.IsTaskAsyncMethod(method))
                {
                    // Clone the method and create a new overload that accepts a CancellationToken
                    CodeMemberMethod memberOverload = method.Clone();
                    memberOverload.PrivateImplementationType = null;
                    memberOverload.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(System.Threading.CancellationToken)), "token"));
                    // Get the indentation level of the original method
                    string indent = new string(' ', 12);
                    // Create the cancellation-related statements with dynamic indentation
                    var throwIfCancelledStmt = new CodeSnippetStatement(indent + "token.ThrowIfCancellationRequested();");
                    // Indentation for the token.Register() block
                    string registerIndent = new string(' ', 16);
                    var registerCancellationStmt = new CodeSnippetStatement(indent +
                        "token.Register(() => { " + Environment.NewLine +
                        registerIndent + "if (base.Channel is IClientChannel channel) { " + Environment.NewLine +
                        registerIndent + "    channel.Abort(); " + Environment.NewLine +
                        registerIndent + "} " + Environment.NewLine +
                        indent + "});");
                    // Insert the cancellation handling logic at the beginning
                    memberOverload.Statements.Insert(0, throwIfCancelledStmt);
                    memberOverload.Statements.Insert(1, registerCancellationStmt);

                    if (memberOverload.Name.Equals("CloseAsync") && memberOverload.Parameters.Count == 1)
                    {
                        CodeIfDirective ifStart = new CodeIfDirective(CodeIfMode.Start, "!NET6_0_OR_GREATER");
                        CodeIfDirective ifEnd = new CodeIfDirective(CodeIfMode.End, "");
                        memberOverload.StartDirectives.Add(ifStart);
                        memberOverload.EndDirectives.Add(ifEnd);
                    }

                    // Insert the new method after the original method
                    type.Members.Insert(idx + 1, memberOverload);
                }                
            }
        }
    }
}
