// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.CodeDom;

namespace Microsoft.Tools.ServiceModel.Svcutil
{
    internal class CodeTypeNameFixer : CodeDomVisitor
    {
        //starting in c#11, types cannot be named as file/required/scoped, work around this by escaping with an @ prefix.
        //ref: https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/breaking-changes/compiler%20breaking%20changes%20-%20dotnet%207#types-cannot-be-named-file
        private readonly List<string> _reservedKeyword = new List<string>() { "file", "required", "scoped" };
        protected override void Visit(CodeCompileUnit cu)
        {
            foreach (CodeNamespace ns in cu.Namespaces)
            {
                foreach (CodeTypeDeclaration typeDeclaration in ns.Types)
                {
                    if(_reservedKeyword.Contains(typeDeclaration.Name))
                    {
                        typeDeclaration.Name = "@" + typeDeclaration.Name;
                    }
                }
            }
        }
    }
}
