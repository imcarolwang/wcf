//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BasicHttp_4_4_0_NS
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BasicHttp_4_4_0_NS.IWcfService_4_4_0")]
    public interface IWcfService_4_4_0
    {
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService_4_4_0/MessageContractRequestReply", ReplyAction="http://tempuri.org/IWcfService_4_4_0/MessageContractRequestReplyResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<BasicHttp_4_4_0_NS.ReplyBankingData_4_4_0> MessageContractRequestReplyAsync(BasicHttp_4_4_0_NS.RequestBankingData_4_4_0 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfService_4_4_0/SendRequestWithXmlElementMessageHeader", ReplyAction="http://tempuri.org/IWcfService_4_4_0/SendRequestWithXmlElementMessageHeaderRespon" +
            "se")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<BasicHttp_4_4_0_NS.XmlElementMessageHeaderResponse> SendRequestWithXmlElementMessageHeaderAsync(BasicHttp_4_4_0_NS.XmlElementMessageHeaderRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [global::System.ServiceModel.MessageContractAttribute(WrapperName="CustomWrapperName", WrapperNamespace="http://www.contoso.com", IsWrapped=true)]
    public partial class RequestBankingData_4_4_0
    {
        
        [global::System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        [global::System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ArrayMultipleElement;
        
        [global::System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        [global::System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [global::System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
        public string[] MultipleElement;
        
        [global::System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        [global::System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string SingleElement;
        
        [global::System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [global::System.Xml.Serialization.XmlElementAttribute(Namespace="http://tempuri.org/")]
        public System.DateTime Date_of_Request;
        
        [global::System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        [global::System.Xml.Serialization.XmlElementAttribute(Namespace="http://tempuri.org/")]
        public decimal Transaction_Amount;
        
        [global::System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.contoso.com", Order=2)]
        [global::System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Customer_Name;
        
        public RequestBankingData_4_4_0()
        {
        }
        
        public RequestBankingData_4_4_0(string ArrayMultipleElement, string[] MultipleElement, string SingleElement, System.DateTime Date_of_Request, decimal Transaction_Amount, string Customer_Name)
        {
            this.ArrayMultipleElement = ArrayMultipleElement;
            this.MultipleElement = MultipleElement;
            this.SingleElement = SingleElement;
            this.Date_of_Request = Date_of_Request;
            this.Transaction_Amount = Transaction_Amount;
            this.Customer_Name = Customer_Name;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [global::System.ServiceModel.MessageContractAttribute(WrapperName="CustomWrapperName", WrapperNamespace="http://www.contoso.com", IsWrapped=true)]
    public partial class ReplyBankingData_4_4_0
    {
        
        [global::System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        [global::System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ArrayMultipleElement;
        
        [global::System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        [global::System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [global::System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
        public string[] MultipleElement;
        
        [global::System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        [global::System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string SingleElement;
        
        [global::System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [global::System.Xml.Serialization.XmlElementAttribute(Namespace="http://tempuri.org/")]
        public System.DateTime Date_of_Request;
        
        [global::System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        [global::System.Xml.Serialization.XmlElementAttribute(Namespace="http://tempuri.org/")]
        public decimal Transaction_Amount;
        
        [global::System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.contoso.com", Order=2)]
        [global::System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Customer_Name;
        
        public ReplyBankingData_4_4_0()
        {
        }
        
        public ReplyBankingData_4_4_0(string ArrayMultipleElement, string[] MultipleElement, string SingleElement, System.DateTime Date_of_Request, decimal Transaction_Amount, string Customer_Name)
        {
            this.ArrayMultipleElement = ArrayMultipleElement;
            this.MultipleElement = MultipleElement;
            this.SingleElement = SingleElement;
            this.Date_of_Request = Date_of_Request;
            this.Transaction_Amount = Transaction_Amount;
            this.Customer_Name = Customer_Name;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [global::System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class XmlElementMessageHeader
    {
        
        private string headerValueField;
        
        /// <remarks/>
        [global::System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string HeaderValue
        {
            get
            {
                return this.headerValueField;
            }
            set
            {
                this.headerValueField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [global::System.ServiceModel.MessageContractAttribute(WrapperName="SendRequestWithXmlElementMessageHeader", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class XmlElementMessageHeaderRequest
    {
        
        [global::System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public BasicHttp_4_4_0_NS.XmlElementMessageHeader TestHeader;
        
        public XmlElementMessageHeaderRequest()
        {
        }
        
        public XmlElementMessageHeaderRequest(BasicHttp_4_4_0_NS.XmlElementMessageHeader TestHeader)
        {
            this.TestHeader = TestHeader;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [global::System.ServiceModel.MessageContractAttribute(WrapperName="SendRequestWithXmlElementMessageHeaderResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class XmlElementMessageHeaderResponse
    {
        
        [global::System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string TestResult;
        
        public XmlElementMessageHeaderResponse()
        {
        }
        
        public XmlElementMessageHeaderResponse(string TestResult)
        {
            this.TestResult = TestResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    public interface IWcfService_4_4_0Channel : BasicHttp_4_4_0_NS.IWcfService_4_4_0, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    public partial class WcfService_4_4_0Client : System.ServiceModel.ClientBase<BasicHttp_4_4_0_NS.IWcfService_4_4_0>, BasicHttp_4_4_0_NS.IWcfService_4_4_0
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public WcfService_4_4_0Client() : 
                base(WcfService_4_4_0Client.GetDefaultBinding(), WcfService_4_4_0Client.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.Basic_IWcfService_4_4_0.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WcfService_4_4_0Client(EndpointConfiguration endpointConfiguration) : 
                base(WcfService_4_4_0Client.GetBindingForEndpoint(endpointConfiguration), WcfService_4_4_0Client.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WcfService_4_4_0Client(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(WcfService_4_4_0Client.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WcfService_4_4_0Client(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(WcfService_4_4_0Client.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WcfService_4_4_0Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<BasicHttp_4_4_0_NS.ReplyBankingData_4_4_0> MessageContractRequestReplyAsync(BasicHttp_4_4_0_NS.RequestBankingData_4_4_0 request)
        {
            return base.Channel.MessageContractRequestReplyAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<BasicHttp_4_4_0_NS.XmlElementMessageHeaderResponse> BasicHttp_4_4_0_NS.IWcfService_4_4_0.SendRequestWithXmlElementMessageHeaderAsync(BasicHttp_4_4_0_NS.XmlElementMessageHeaderRequest request)
        {
            return base.Channel.SendRequestWithXmlElementMessageHeaderAsync(request);
        }
        
        public System.Threading.Tasks.Task<BasicHttp_4_4_0_NS.XmlElementMessageHeaderResponse> SendRequestWithXmlElementMessageHeaderAsync(BasicHttp_4_4_0_NS.XmlElementMessageHeader TestHeader)
        {
            BasicHttp_4_4_0_NS.XmlElementMessageHeaderRequest inValue = new BasicHttp_4_4_0_NS.XmlElementMessageHeaderRequest();
            inValue.TestHeader = TestHeader;
            return ((BasicHttp_4_4_0_NS.IWcfService_4_4_0)(this)).SendRequestWithXmlElementMessageHeaderAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.Basic_IWcfService_4_4_0))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = global::System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.Basic_IWcfService_4_4_0))
            {
                return new System.ServiceModel.EndpointAddress("http://wcfcoresrv53.westus3.cloudapp.azure.com/WcfTestService1/BasicHttp_4_4_0.sv" +
                        "c/Basic");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return WcfService_4_4_0Client.GetBindingForEndpoint(EndpointConfiguration.Basic_IWcfService_4_4_0);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return WcfService_4_4_0Client.GetEndpointAddress(EndpointConfiguration.Basic_IWcfService_4_4_0);
        }
        
        public enum EndpointConfiguration
        {
            
            Basic_IWcfService_4_4_0,
        }
    }
}