// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace System.ServiceModel.Security.Tokens
{
    public class IssuedSecurityTokenParameters : SecurityTokenParameters
    {
        internal const SecurityKeyType DefaultKeyType = SecurityKeyType.SymmetricKey;
        internal const bool DefaultUseStrTransform = false;
        const string WsidNamespace = "http://schemas.xmlsoap.org/ws/2005/05/identity";
        static readonly string s_wsidPPIClaim = string.Format(CultureInfo.InvariantCulture, "{0}/claims/privatepersonalidentifier", WsidNamespace);

        internal struct AlternativeIssuerEndpoint
        {
            public EndpointAddress IssuerAddress;
            public EndpointAddress IssuerMetadataAddress;
            public Binding IssuerBinding;
        }

        int _keySize;
        SecurityKeyType _keyType = DefaultKeyType;

        protected IssuedSecurityTokenParameters(IssuedSecurityTokenParameters other)
            : base(other)
        {
            DefaultMessageSecurityVersion = other.DefaultMessageSecurityVersion;
            IssuerAddress = other.IssuerAddress;
            _keyType = other._keyType;
            TokenType = other.TokenType;
            _keySize = other._keySize;
            UseStrTransform = other.UseStrTransform;

            foreach (XmlElement parameter in other.AdditionalRequestParameters)
            {
                AdditionalRequestParameters.Add((XmlElement)parameter.Clone());
            }
            foreach (ClaimTypeRequirement c in other.ClaimTypeRequirements)
            {
                ClaimTypeRequirements.Add(c);
            }
            if (other.IssuerBinding != null)
            {
                IssuerBinding = new CustomBinding(other.IssuerBinding);
            }
            IssuerMetadataAddress = other.IssuerMetadataAddress;
        }

        public IssuedSecurityTokenParameters()
            : this(null, null, null)
        {
            // empty
        }

        public IssuedSecurityTokenParameters(string tokenType)
            : this(tokenType, null, null)
        {
            // empty
        }

        public IssuedSecurityTokenParameters(string tokenType, EndpointAddress issuerAddress)
            : this(tokenType, issuerAddress, null)
        {
            // empty
        }

        public IssuedSecurityTokenParameters(string tokenType, EndpointAddress issuerAddress, Binding issuerBinding)
            : base()
        {
            TokenType = tokenType;
            IssuerAddress = issuerAddress;
            IssuerBinding = issuerBinding;
        }

        internal static IssuedSecurityTokenParameters CreateInfoCardParameters(SecurityStandardsManager standardsManager, SecurityAlgorithmSuite algorithm)
        {
            IssuedSecurityTokenParameters result = new IssuedSecurityTokenParameters(SecurityXXX2005Strings.SamlTokenType);
            result.KeyType = SecurityKeyType.AsymmetricKey;
            result.ClaimTypeRequirements.Add(new ClaimTypeRequirement(s_wsidPPIClaim));
            result.IssuerAddress = null;
            result.AddAlgorithmParameters(algorithm, standardsManager, result.KeyType);
            return result;
        }

        internal void AddAlgorithmParameters(SecurityAlgorithmSuite algorithmSuite, SecurityStandardsManager standardsManager, SecurityKeyType issuedKeyType)
        {
            this.AdditionalRequestParameters.Insert(0, standardsManager.TrustDriver.CreateEncryptionAlgorithmElement(algorithmSuite.DefaultEncryptionAlgorithm));
            this.AdditionalRequestParameters.Insert(0, standardsManager.TrustDriver.CreateCanonicalizationAlgorithmElement(algorithmSuite.DefaultCanonicalizationAlgorithm));

            if (this.KeyType == SecurityKeyType.BearerKey)
            {
                // As the client does not have a proof token in the Bearer case
                // we don't have any specific algorithms to request for.
                return;
            }

            string signWithAlgorithm = (this.KeyType == SecurityKeyType.SymmetricKey) ? algorithmSuite.DefaultSymmetricSignatureAlgorithm : algorithmSuite.DefaultAsymmetricSignatureAlgorithm;
            this.AdditionalRequestParameters.Insert(0, standardsManager.TrustDriver.CreateSignWithElement(signWithAlgorithm));
            string encryptWithAlgorithm;
            if (issuedKeyType == SecurityKeyType.SymmetricKey)
            {
                encryptWithAlgorithm = algorithmSuite.DefaultEncryptionAlgorithm;
            }
            else
            {
                encryptWithAlgorithm = algorithmSuite.DefaultAsymmetricKeyWrapAlgorithm;
            }
            this.AdditionalRequestParameters.Insert(0, standardsManager.TrustDriver.CreateEncryptWithElement(encryptWithAlgorithm));

            if (standardsManager.MessageSecurityVersion.TrustVersion != TrustVersion.WSTrustFeb2005)
            {
                this.AdditionalRequestParameters.Insert(0, ((WSTrustDec2005.DriverDec2005)standardsManager.TrustDriver).CreateKeyWrapAlgorithmElement(algorithmSuite.DefaultAsymmetricKeyWrapAlgorithm));
            }

            return;
        }

        internal protected override bool HasAsymmetricKey { get { return KeyType == SecurityKeyType.AsymmetricKey; } }

        public Collection<XmlElement> AdditionalRequestParameters { get; } = new Collection<XmlElement>();

        public MessageSecurityVersion DefaultMessageSecurityVersion { get; set; }

        internal Collection<AlternativeIssuerEndpoint> AlternativeIssuerEndpoints { get; } = new Collection<AlternativeIssuerEndpoint>();

        public EndpointAddress IssuerAddress { get; set; }

        public EndpointAddress IssuerMetadataAddress { get; set; }

        public Binding IssuerBinding { get; set; }

        public SecurityKeyType KeyType
        {
            get
            {
                return _keyType;
            }
            set
            {
                SecurityKeyTypeHelper.Validate(value);
                _keyType = value;
            }
        }

        public int KeySize
        {
            get
            {
                return _keySize;
            }
            set
            {
                if (value < 0)
                {
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException(nameof(value), SRP.ValueMustBeNonNegative));
                }

                _keySize = value;
            }
        }

        public bool UseStrTransform { get; set; } = DefaultUseStrTransform;

        public Collection<ClaimTypeRequirement> ClaimTypeRequirements { get; } = new Collection<ClaimTypeRequirement>();

        public string TokenType { get; set; }

        internal protected override bool SupportsClientAuthentication { get { return true; } }
        internal protected override bool SupportsServerAuthentication { get { return true; } }
        internal protected override bool SupportsClientWindowsIdentity { get { return false; } }

        protected override SecurityTokenParameters CloneCore()
        {
            return new IssuedSecurityTokenParameters(this);
        }

        internal protected override SecurityKeyIdentifierClause CreateKeyIdentifierClause(SecurityToken token, SecurityTokenReferenceStyle referenceStyle)
        {
            if (token is GenericXmlSecurityToken)
                return base.CreateGenericXmlTokenKeyIdentifierClause(token, referenceStyle);
            else
                throw ExceptionHelper.PlatformNotSupported();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());

            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "TokenType: {0}", TokenType == null ? "null" : TokenType));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "KeyType: {0}", _keyType.ToString()));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "KeySize: {0}", _keySize.ToString(CultureInfo.InvariantCulture)));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "IssuerAddress: {0}", IssuerAddress == null ? "null" : IssuerAddress.ToString()));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "IssuerMetadataAddress: {0}", IssuerMetadataAddress == null ? "null" : IssuerMetadataAddress.ToString()));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "DefaultMessgeSecurityVersion: {0}", DefaultMessageSecurityVersion == null ? "null" : DefaultMessageSecurityVersion.ToString()));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "UseStrTransform: {0}", UseStrTransform.ToString()));

            if (IssuerBinding == null)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "IssuerBinding: null"));
            }
            else
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "IssuerBinding:"));
                BindingElementCollection bindingElements = IssuerBinding.CreateBindingElements();
                for (int i = 0; i < bindingElements.Count; i++)
                {
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "  BindingElement[{0}]:", i.ToString(CultureInfo.InvariantCulture)));
                    sb.AppendLine("    " + bindingElements[i].ToString().Trim().Replace("\n", "\n    "));
                }
            }

            if (ClaimTypeRequirements.Count == 0)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "ClaimTypeRequirements: none"));
            }
            else
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "ClaimTypeRequirements:"));
                for (int i = 0; i < ClaimTypeRequirements.Count; i++)
                {
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "  {0}, optional={1}", ClaimTypeRequirements[i].ClaimType, ClaimTypeRequirements[i].IsOptional));
                }
            }

            return sb.ToString().Trim();
        }

        protected internal override void InitializeSecurityTokenRequirement(SecurityTokenRequirement requirement)
        {
            requirement.TokenType = TokenType;
            requirement.RequireCryptographicToken = true;
            requirement.KeyType = KeyType;

            ServiceModelSecurityTokenRequirement serviceModelSecurityTokenRequirement = requirement as ServiceModelSecurityTokenRequirement;
            if (serviceModelSecurityTokenRequirement != null)
            {
                serviceModelSecurityTokenRequirement.DefaultMessageSecurityVersion = DefaultMessageSecurityVersion;
            }
            else
            {
                requirement.Properties[ServiceModelSecurityTokenRequirement.DefaultMessageSecurityVersionProperty] = DefaultMessageSecurityVersion;
            }

            if (KeySize > 0)
            {
                requirement.KeySize = KeySize;
            }
            requirement.Properties[ServiceModelSecurityTokenRequirement.IssuerAddressProperty] = IssuerAddress;
            if (IssuerBinding != null)
            {
                requirement.Properties[ServiceModelSecurityTokenRequirement.IssuerBindingProperty] = IssuerBinding;
            }
            requirement.Properties[ServiceModelSecurityTokenRequirement.IssuedSecurityTokenParametersProperty] = Clone();
        }
    }
}
