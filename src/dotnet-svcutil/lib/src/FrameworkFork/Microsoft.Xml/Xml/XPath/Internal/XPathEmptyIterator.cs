// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace MS.Internal.Xml.XPath
{
    using System;
    using Microsoft.Xml;
    using Microsoft.Xml.XPath;
    using System.Diagnostics;
    using System.Globalization;
    using System.Collections;

    internal sealed class XPathEmptyIterator : ResetableIterator
    {
        private XPathEmptyIterator() { }
        public override XPathNodeIterator Clone() { return this; }

        public override XPathNavigator Current
        {
            get { return null; }
        }

        public override int CurrentPosition
        {
            get { return 0; }
        }

        public override int Count
        {
            get { return 0; }
        }

        public override bool MoveNext()
        {
            return false;
        }

        public override void Reset() { }

        // -- Instance
        public static XPathEmptyIterator Instance = new XPathEmptyIterator();
    }
}
