//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BasicHttpRpcLitDualNs_NS
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [global::System.Runtime.Serialization.DataContractAttribute(Name="IntParams", Namespace="http://contoso.com/calc")]
    public partial class IntParams : object
    {
        
        private int P1Field;
        
        private int P2Field;
        
        [global::System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int P1
        {
            get
            {
                return this.P1Field;
            }
            set
            {
                this.P1Field = value;
            }
        }
        
        [global::System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int P2
        {
            get
            {
                return this.P2Field;
            }
            set
            {
                this.P2Field = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [global::System.Runtime.Serialization.DataContractAttribute(Name="FloatParams", Namespace="http://contoso.com/calc")]
    public partial class FloatParams : object
    {
        
        private float P1Field;
        
        private float P2Field;
        
        [global::System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public float P1
        {
            get
            {
                return this.P1Field;
            }
            set
            {
                this.P1Field = value;
            }
        }
        
        [global::System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public float P2
        {
            get
            {
                return this.P2Field;
            }
            set
            {
                this.P2Field = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [global::System.Runtime.Serialization.DataContractAttribute(Name="ByteParams", Namespace="http://contoso.com/calc")]
    public partial class ByteParams : object
    {
        
        private byte P1Field;
        
        private byte P2Field;
        
        [global::System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public byte P1
        {
            get
            {
                return this.P1Field;
            }
            set
            {
                this.P1Field = value;
            }
        }
        
        [global::System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public byte P2
        {
            get
            {
                return this.P2Field;
            }
            set
            {
                this.P2Field = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://contoso.com/calc", ConfigurationName="BasicHttpRpcLitDualNs_NS.ICalculatorRpcLit")]
    public interface ICalculatorRpcLit
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcLit/Sum2", ReplyAction="http://contoso.com/calc/ICalculatorRpcLit/Sum2Response")]
        [System.ServiceModel.DataContractFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc)]
        System.Threading.Tasks.Task<int> Sum2Async(int i, int j);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcLit/Sum", ReplyAction="http://contoso.com/calc/ICalculatorRpcLit/SumResponse")]
        [System.ServiceModel.DataContractFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc)]
        System.Threading.Tasks.Task<int> SumAsync(BasicHttpRpcLitDualNs_NS.IntParams par);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcLit/Divide", ReplyAction="http://contoso.com/calc/ICalculatorRpcLit/DivideResponse")]
        [System.ServiceModel.DataContractFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc)]
        System.Threading.Tasks.Task<float> DivideAsync(BasicHttpRpcLitDualNs_NS.FloatParams par);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcLit/Concatenate", ReplyAction="http://contoso.com/calc/ICalculatorRpcLit/ConcatenateResponse")]
        [System.ServiceModel.DataContractFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc)]
        System.Threading.Tasks.Task<string> ConcatenateAsync(BasicHttpRpcLitDualNs_NS.IntParams par);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcLit/AddIntParams", ReplyAction="http://contoso.com/calc/ICalculatorRpcLit/AddIntParamsResponse")]
        [System.ServiceModel.DataContractFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc)]
        System.Threading.Tasks.Task AddIntParamsAsync(string guid, BasicHttpRpcLitDualNs_NS.IntParams par);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcLit/GetAndRemoveIntParams", ReplyAction="http://contoso.com/calc/ICalculatorRpcLit/GetAndRemoveIntParamsResponse")]
        [System.ServiceModel.DataContractFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc)]
        System.Threading.Tasks.Task<BasicHttpRpcLitDualNs_NS.IntParams> GetAndRemoveIntParamsAsync(string guid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcLit/ReturnInputDateTime", ReplyAction="http://contoso.com/calc/ICalculatorRpcLit/ReturnInputDateTimeResponse")]
        [System.ServiceModel.DataContractFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc)]
        System.Threading.Tasks.Task<System.DateTime> ReturnInputDateTimeAsync(System.DateTime dt);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://contoso.com/calc/ICalculatorRpcLit/CreateSet", ReplyAction="http://contoso.com/calc/ICalculatorRpcLit/CreateSetResponse")]
        [System.ServiceModel.DataContractFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc)]
        System.Threading.Tasks.Task<byte[]> CreateSetAsync(BasicHttpRpcLitDualNs_NS.ByteParams par);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    public interface ICalculatorRpcLitChannel : BasicHttpRpcLitDualNs_NS.ICalculatorRpcLit, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    public partial class CalculatorRpcLitClient : System.ServiceModel.ClientBase<BasicHttpRpcLitDualNs_NS.ICalculatorRpcLit>, BasicHttpRpcLitDualNs_NS.ICalculatorRpcLit
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public CalculatorRpcLitClient() : 
                base(CalculatorRpcLitClient.GetDefaultBinding(), CalculatorRpcLitClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.Basic_ICalculatorRpcLit.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CalculatorRpcLitClient(EndpointConfiguration endpointConfiguration) : 
                base(CalculatorRpcLitClient.GetBindingForEndpoint(endpointConfiguration), CalculatorRpcLitClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CalculatorRpcLitClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(CalculatorRpcLitClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CalculatorRpcLitClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(CalculatorRpcLitClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CalculatorRpcLitClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<int> Sum2Async(int i, int j)
        {
            return base.Channel.Sum2Async(i, j);
        }
        
        public System.Threading.Tasks.Task<int> SumAsync(BasicHttpRpcLitDualNs_NS.IntParams par)
        {
            return base.Channel.SumAsync(par);
        }
        
        public System.Threading.Tasks.Task<float> DivideAsync(BasicHttpRpcLitDualNs_NS.FloatParams par)
        {
            return base.Channel.DivideAsync(par);
        }
        
        public System.Threading.Tasks.Task<string> ConcatenateAsync(BasicHttpRpcLitDualNs_NS.IntParams par)
        {
            return base.Channel.ConcatenateAsync(par);
        }
        
        public System.Threading.Tasks.Task AddIntParamsAsync(string guid, BasicHttpRpcLitDualNs_NS.IntParams par)
        {
            return base.Channel.AddIntParamsAsync(guid, par);
        }
        
        public System.Threading.Tasks.Task<BasicHttpRpcLitDualNs_NS.IntParams> GetAndRemoveIntParamsAsync(string guid)
        {
            return base.Channel.GetAndRemoveIntParamsAsync(guid);
        }
        
        public System.Threading.Tasks.Task<System.DateTime> ReturnInputDateTimeAsync(System.DateTime dt)
        {
            return base.Channel.ReturnInputDateTimeAsync(dt);
        }
        
        public System.Threading.Tasks.Task<byte[]> CreateSetAsync(BasicHttpRpcLitDualNs_NS.ByteParams par)
        {
            return base.Channel.CreateSetAsync(par);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.Basic_ICalculatorRpcLit))
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
            if ((endpointConfiguration == EndpointConfiguration.Basic_ICalculatorRpcLit))
            {
                return new System.ServiceModel.EndpointAddress("http://wcfcoresrv53.westus3.cloudapp.azure.com/WcfTestService1/BasicHttpRpcLitDua" +
                        "lNs.svc/Basic");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return CalculatorRpcLitClient.GetBindingForEndpoint(EndpointConfiguration.Basic_ICalculatorRpcLit);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return CalculatorRpcLitClient.GetEndpointAddress(EndpointConfiguration.Basic_ICalculatorRpcLit);
        }
        
        public enum EndpointConfiguration
        {
            
            Basic_ICalculatorRpcLit,
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BasicHttpRpcLitDualNs_NS.IHelloWorldRpcLit")]
    public interface IHelloWorldRpcLit
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHelloWorldRpcLit/AddString", ReplyAction="http://tempuri.org/IHelloWorldRpcLit/AddStringResponse")]
        [System.ServiceModel.DataContractFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc)]
        System.Threading.Tasks.Task AddStringAsync(string guid, string testString);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IHelloWorldRpcLit/GetAndRemoveString", ReplyAction="http://tempuri.org/IHelloWorldRpcLit/GetAndRemoveStringResponse")]
        [System.ServiceModel.DataContractFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc)]
        System.Threading.Tasks.Task<string> GetAndRemoveStringAsync(string guid);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    public interface IHelloWorldRpcLitChannel : BasicHttpRpcLitDualNs_NS.IHelloWorldRpcLit, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "99.99.99")]
    public partial class HelloWorldRpcLitClient : System.ServiceModel.ClientBase<BasicHttpRpcLitDualNs_NS.IHelloWorldRpcLit>, BasicHttpRpcLitDualNs_NS.IHelloWorldRpcLit
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public HelloWorldRpcLitClient() : 
                base(HelloWorldRpcLitClient.GetDefaultBinding(), HelloWorldRpcLitClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.Basic_IHelloWorldRpcLit.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public HelloWorldRpcLitClient(EndpointConfiguration endpointConfiguration) : 
                base(HelloWorldRpcLitClient.GetBindingForEndpoint(endpointConfiguration), HelloWorldRpcLitClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public HelloWorldRpcLitClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(HelloWorldRpcLitClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public HelloWorldRpcLitClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(HelloWorldRpcLitClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public HelloWorldRpcLitClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task AddStringAsync(string guid, string testString)
        {
            return base.Channel.AddStringAsync(guid, testString);
        }
        
        public System.Threading.Tasks.Task<string> GetAndRemoveStringAsync(string guid)
        {
            return base.Channel.GetAndRemoveStringAsync(guid);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.Basic_IHelloWorldRpcLit))
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
            if ((endpointConfiguration == EndpointConfiguration.Basic_IHelloWorldRpcLit))
            {
                return new System.ServiceModel.EndpointAddress("http://wcfcoresrv53.westus3.cloudapp.azure.com/WcfTestService1/BasicHttpRpcLitDua" +
                        "lNs.svc/Basic");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return HelloWorldRpcLitClient.GetBindingForEndpoint(EndpointConfiguration.Basic_IHelloWorldRpcLit);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return HelloWorldRpcLitClient.GetEndpointAddress(EndpointConfiguration.Basic_IHelloWorldRpcLit);
        }
        
        public enum EndpointConfiguration
        {
            
            Basic_IHelloWorldRpcLit,
        }
    }
}