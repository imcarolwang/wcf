//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BasicHttpRpcEncDualNs_NS
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://contoso.com/calc", ConfigurationName="BasicHttpRpcEncDualNs_NS.ICalculatorRpcEnc")]
    public interface ICalculatorRpcEnc
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcEnc/Sum2", ReplyAction="http://contoso.com/calc/ICalculatorRpcEnc/Sum2Response")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        System.Threading.Tasks.Task<int> Sum2Async(int i, int j);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcEnc/Sum", ReplyAction="http://contoso.com/calc/ICalculatorRpcEnc/SumResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        System.Threading.Tasks.Task<int> SumAsync(BasicHttpRpcEncDualNs_NS.IntParams par);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcEnc/Divide", ReplyAction="http://contoso.com/calc/ICalculatorRpcEnc/DivideResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        System.Threading.Tasks.Task<float> DivideAsync(BasicHttpRpcEncDualNs_NS.FloatParams par);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcEnc/Concatenate", ReplyAction="http://contoso.com/calc/ICalculatorRpcEnc/ConcatenateResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        System.Threading.Tasks.Task<string> ConcatenateAsync(BasicHttpRpcEncDualNs_NS.IntParams par);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcEnc/AddIntParams", ReplyAction="http://contoso.com/calc/ICalculatorRpcEnc/AddIntParamsResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        System.Threading.Tasks.Task AddIntParamsAsync(System.Guid guid, BasicHttpRpcEncDualNs_NS.IntParams par);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcEnc/GetAndRemoveIntParams", ReplyAction="http://contoso.com/calc/ICalculatorRpcEnc/GetAndRemoveIntParamsResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        System.Threading.Tasks.Task<BasicHttpRpcEncDualNs_NS.IntParams> GetAndRemoveIntParamsAsync(System.Guid guid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcEnc/ReturnInputDateTime", ReplyAction="http://contoso.com/calc/ICalculatorRpcEnc/ReturnInputDateTimeResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        System.Threading.Tasks.Task<System.DateTime> ReturnInputDateTimeAsync(System.DateTime dt);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcEnc/CreateSet", ReplyAction="http://contoso.com/calc/ICalculatorRpcEnc/CreateSetResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.Xml.Serialization.SoapElementAttribute(DataType="base64Binary")]
        System.Threading.Tasks.Task<byte[]> CreateSetAsync(BasicHttpRpcEncDualNs_NS.ByteParams par);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://contoso.com/calc/encoded")]
    public partial class IntParams
    {
        
        private int p1Field;
        
        private int p2Field;
        
        /// <remarks/>
        public int P1
        {
            get
            {
                return this.p1Field;
            }
            set
            {
                this.p1Field = value;
            }
        }
        
        /// <remarks/>
        public int P2
        {
            get
            {
                return this.p2Field;
            }
            set
            {
                this.p2Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://contoso.com/calc/encoded")]
    public partial class ByteParams
    {
        
        private byte p1Field;
        
        private byte p2Field;
        
        /// <remarks/>
        public byte P1
        {
            get
            {
                return this.p1Field;
            }
            set
            {
                this.p1Field = value;
            }
        }
        
        /// <remarks/>
        public byte P2
        {
            get
            {
                return this.p2Field;
            }
            set
            {
                this.p2Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://contoso.com/calc/encoded")]
    public partial class FloatParams
    {
        
        private float p1Field;
        
        private float p2Field;
        
        /// <remarks/>
        public float P1
        {
            get
            {
                return this.p1Field;
            }
            set
            {
                this.p1Field = value;
            }
        }
        
        /// <remarks/>
        public float P2
        {
            get
            {
                return this.p2Field;
            }
            set
            {
                this.p2Field = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    public interface ICalculatorRpcEncChannel : BasicHttpRpcEncDualNs_NS.ICalculatorRpcEnc, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    public partial class CalculatorRpcEncClient : System.ServiceModel.ClientBase<BasicHttpRpcEncDualNs_NS.ICalculatorRpcEnc>, BasicHttpRpcEncDualNs_NS.ICalculatorRpcEnc
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public CalculatorRpcEncClient() : 
                base(CalculatorRpcEncClient.GetDefaultBinding(), CalculatorRpcEncClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.Basic_ICalculatorRpcEnc.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CalculatorRpcEncClient(EndpointConfiguration endpointConfiguration) : 
                base(CalculatorRpcEncClient.GetBindingForEndpoint(endpointConfiguration), CalculatorRpcEncClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CalculatorRpcEncClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(CalculatorRpcEncClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CalculatorRpcEncClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(CalculatorRpcEncClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CalculatorRpcEncClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<int> Sum2Async(int i, int j)
        {
            return base.Channel.Sum2Async(i, j);
        }
        
        public System.Threading.Tasks.Task<int> SumAsync(BasicHttpRpcEncDualNs_NS.IntParams par)
        {
            return base.Channel.SumAsync(par);
        }
        
        public System.Threading.Tasks.Task<float> DivideAsync(BasicHttpRpcEncDualNs_NS.FloatParams par)
        {
            return base.Channel.DivideAsync(par);
        }
        
        public System.Threading.Tasks.Task<string> ConcatenateAsync(BasicHttpRpcEncDualNs_NS.IntParams par)
        {
            return base.Channel.ConcatenateAsync(par);
        }
        
        public System.Threading.Tasks.Task AddIntParamsAsync(System.Guid guid, BasicHttpRpcEncDualNs_NS.IntParams par)
        {
            return base.Channel.AddIntParamsAsync(guid, par);
        }
        
        public System.Threading.Tasks.Task<BasicHttpRpcEncDualNs_NS.IntParams> GetAndRemoveIntParamsAsync(System.Guid guid)
        {
            return base.Channel.GetAndRemoveIntParamsAsync(guid);
        }
        
        public System.Threading.Tasks.Task<System.DateTime> ReturnInputDateTimeAsync(System.DateTime dt)
        {
            return base.Channel.ReturnInputDateTimeAsync(dt);
        }
        
        public System.Threading.Tasks.Task<byte[]> CreateSetAsync(BasicHttpRpcEncDualNs_NS.ByteParams par)
        {
            return base.Channel.CreateSetAsync(par);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        #if !NET6_0_OR_GREATER
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        #endif
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.Basic_ICalculatorRpcEnc))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.Basic_ICalculatorRpcEnc))
            {
                return new System.ServiceModel.EndpointAddress("http://wcfcoresrv53.westus3.cloudapp.azure.com/WcfTestService1/BasicHttpRpcEncDua" +
                        "lNs.svc/Basic");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return CalculatorRpcEncClient.GetBindingForEndpoint(EndpointConfiguration.Basic_ICalculatorRpcEnc);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return CalculatorRpcEncClient.GetEndpointAddress(EndpointConfiguration.Basic_ICalculatorRpcEnc);
        }
        
        public enum EndpointConfiguration
        {
            
            Basic_ICalculatorRpcEnc,
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BasicHttpRpcEncDualNs_NS.IHelloWorldRpcEnc")]
    public interface IHelloWorldRpcEnc
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHelloWorldRpcEnc/AddString", ReplyAction="http://tempuri.org/IHelloWorldRpcEnc/AddStringResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        System.Threading.Tasks.Task AddStringAsync(System.Guid guid, string testString);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHelloWorldRpcEnc/GetAndRemoveString", ReplyAction="http://tempuri.org/IHelloWorldRpcEnc/GetAndRemoveStringResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        System.Threading.Tasks.Task<string> GetAndRemoveStringAsync(System.Guid guid);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    public interface IHelloWorldRpcEncChannel : BasicHttpRpcEncDualNs_NS.IHelloWorldRpcEnc, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    public partial class HelloWorldRpcEncClient : System.ServiceModel.ClientBase<BasicHttpRpcEncDualNs_NS.IHelloWorldRpcEnc>, BasicHttpRpcEncDualNs_NS.IHelloWorldRpcEnc
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public HelloWorldRpcEncClient() : 
                base(HelloWorldRpcEncClient.GetDefaultBinding(), HelloWorldRpcEncClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.Basic_IHelloWorldRpcEnc.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public HelloWorldRpcEncClient(EndpointConfiguration endpointConfiguration) : 
                base(HelloWorldRpcEncClient.GetBindingForEndpoint(endpointConfiguration), HelloWorldRpcEncClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public HelloWorldRpcEncClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(HelloWorldRpcEncClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public HelloWorldRpcEncClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(HelloWorldRpcEncClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public HelloWorldRpcEncClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task AddStringAsync(System.Guid guid, string testString)
        {
            return base.Channel.AddStringAsync(guid, testString);
        }
        
        public System.Threading.Tasks.Task<string> GetAndRemoveStringAsync(System.Guid guid)
        {
            return base.Channel.GetAndRemoveStringAsync(guid);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        #if !NET6_0_OR_GREATER
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        #endif
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.Basic_IHelloWorldRpcEnc))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.Basic_IHelloWorldRpcEnc))
            {
                return new System.ServiceModel.EndpointAddress("http://wcfcoresrv53.westus3.cloudapp.azure.com/WcfTestService1/BasicHttpRpcEncDua" +
                        "lNs.svc/Basic");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return HelloWorldRpcEncClient.GetBindingForEndpoint(EndpointConfiguration.Basic_IHelloWorldRpcEnc);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return HelloWorldRpcEncClient.GetEndpointAddress(EndpointConfiguration.Basic_IHelloWorldRpcEnc);
        }
        
        public enum EndpointConfiguration
        {
            
            Basic_IHelloWorldRpcEnc,
        }
    }
}