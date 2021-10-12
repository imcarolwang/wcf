// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.ServiceModel.Channels
{
    using System.ServiceModel.Description;
    using System.Xml;


    public sealed class PrivacyNoticeBindingElement : BindingElement, IPolicyExportExtension
    {
        private Uri _url;
        private int _version;

        public PrivacyNoticeBindingElement()
        {
            _url = null;
        }

        public PrivacyNoticeBindingElement(PrivacyNoticeBindingElement elementToBeCloned)
            : base(elementToBeCloned)
        {
            _url = elementToBeCloned._url;
            _version = elementToBeCloned._version;
        }

        public Uri Url
        {
            get
            {
                return _url;
            }
            set
            {
                if (value == null)
                {
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
                }

                _url = value;
            }
        }

        public int Version
        {
            get
            {
                return _version;
            }
            set
            {
                if (value < 0)
                {
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("value", value, SRServiceModel.ValueMustBePositive));
                }
                _version = value;
            }
        }

        public override BindingElement Clone()
        {
            return new PrivacyNoticeBindingElement(this);
        }

        public override T GetProperty<T>(BindingContext context)
        {
            if (context == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("context");
            }
            return context.GetInnerProperty<T>();
        }

        void IPolicyExportExtension.ExportPolicy(MetadataExporter exporter, PolicyConversionContext context)
        {
            if (context == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("context");

            if (context.BindingElements != null)
            {
                PrivacyNoticeBindingElement settings =
                    context.BindingElements.Find<PrivacyNoticeBindingElement>();

                if (settings != null)
                {
                    XmlDocument doc = new XmlDocument();

                    // PrivacyNotice assertion
                    XmlElement assertion = doc.CreateElement(PrivacyNoticePolicyStrings.PrivacyNoticePrefix,
                                                              PrivacyNoticePolicyStrings.PrivacyNoticeName,
                                                              PrivacyNoticePolicyStrings.PrivacyNoticeNamespace);

                    assertion.InnerText = settings.Url.ToString();
                    assertion.SetAttribute(PrivacyNoticePolicyStrings.PrivacyNoticeVersionAttributeName, PrivacyNoticePolicyStrings.PrivacyNoticeNamespace, XmlConvert.ToString(settings.Version));

                    context.GetBindingAssertions().Add(assertion);
                }
            }
        }

        internal override bool IsMatch(BindingElement b)
        {
            if (b == null)
                return false;
            PrivacyNoticeBindingElement privacy = b as PrivacyNoticeBindingElement;
            if (privacy == null)
                return false;
            return (_url == privacy._url && _version == privacy._version);
        }
    }
}
