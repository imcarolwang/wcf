// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System.ServiceModel;
using Infrastructure.Common;
using Xunit;
using System;

public static partial class XmlSerializerFormatTests
{
    [WcfFact]
    [OuterLoop]
    public static void XmlSerializerFormatAttribute_SupportFaults()
    {
        BasicHttpBinding binding = null;
        EndpointAddress endpointAddress = null;
        ChannelFactory<IXmlSFAttribute> factory = null;
        IXmlSFAttribute serviceProxy = null;

        // *** SETUP *** \\
        binding = new BasicHttpBinding();
        endpointAddress = new EndpointAddress(Endpoints.XmlSFAttribute_Address);
        factory = new ChannelFactory<IXmlSFAttribute>(binding, endpointAddress);
        serviceProxy = factory.CreateChannel();

        // *** EXECUTE 1st Variation *** \\
        try
        {
            // Calling the Operation Contract overload with "SupportFaults" not set, default is "false"
            serviceProxy.TestXmlSerializerSupportsFaults_False();
        }
        catch (FaultException<FaultDetailWithXmlSerializerFormatAttribute> fException)
        {
            // In this variation the Fault message should have been returned using the Data Contract Serializer.
            Assert.True(fException.Detail.UsedDataContractSerializer, "The returning Fault Detail should have used the Data Contract Serializer.");
            Assert.True(fException.Detail.UsedXmlSerializer == false, "The returning Fault Detail should NOT have used the Xml Serializer.");
        }
        catch (Exception exception)
        {
            Assert.True(false, $"Test Failed, caught unexpected exception.\nException: {exception.ToString()}\nException Message: {exception.Message}");
        }
        finally
        {
            // *** ENSURE CLEANUP *** \\
            ScenarioTestHelpers.CloseCommunicationObjects((ICommunicationObject)serviceProxy);
        }

        // *** EXECUTE 2nd Variation *** \\
        try
        {
            serviceProxy = factory.CreateChannel();
            serviceProxy.TestXmlSerializerSupportsFaults_True();
        }
        catch (FaultException<FaultDetailWithXmlSerializerFormatAttribute> fException)
        {
            // In this variation the Fault message should have been returned using the Xml Serializer.
            Assert.True(fException.Detail.UsedDataContractSerializer == false, "The returning Fault Detail should NOT have used the Data Contract Serializer.");
            Assert.True(fException.Detail.UsedXmlSerializer, "The returning Fault Detail should have used the Xml Serializer.");
        }
        catch (Exception exception)
        {
            Assert.True(false, $"Test Failed, caught unexpected exception.\nException: {exception.ToString()}\nException Message: {exception.Message}");
        }
        finally
        {
            // *** ENSURE CLEANUP *** \\
            ScenarioTestHelpers.CloseCommunicationObjects((ICommunicationObject)serviceProxy, factory);
        }
    }

    [WcfFact]
    [OuterLoop]
    public static void XmlSFAttributeRpcEncSingleNsTest()
    {
        RunVariation(Endpoints.BasciHttpRpcEncSingleNs_Address);
    }

    public static void XmlSFAttributeRpcLitSingleNsTest()
    {
        RunVariation(Endpoints.BasicHttpRpcLitSingleNs_Address);
    }

    public static void XmlSFAttributeDocLitSingleNsTest()
    {
        RunVariation(Endpoints.BasicHttpDocLitSingleNs_Address);        
    }

    public static void XmlSFAttributeRpcEncMultiNTest()
    {
        RunVariation(Endpoints.BasicHttpRpcEncMultiNs_Address, true);
    }

    public static void XmlSFAttributeRpcLitMultiNsTest()
    {
        RunVariation(Endpoints.BasicHttpRpcLitMultiNs_Address, true);
    }

    public static void XmlSFAttributeDocLitMultiNsTest()
    {
        RunVariation(Endpoints.BasicHttpDocLitMultiNs_Address, true);
    }

    private static void RunVariation(string serviceAddress, bool isMultiNs = false)
    {
        BasicHttpBinding binding = null;
        EndpointAddress endpointAddress = null;
        ChannelFactory<ICalculator> factory1 = null;
        ChannelFactory<IHelloWorld> factory2 = null;
        ICalculator serviceProxy1 = null;
        IHelloWorld serviceProxy2 = null;

        // *** SETUP *** \\
        binding = new BasicHttpBinding();
        endpointAddress = new EndpointAddress(serviceAddress);
        factory1 = new ChannelFactory<ICalculator>(binding, endpointAddress);
        serviceProxy1 = factory1.CreateChannel();
        if (isMultiNs)
        {
            factory2 = new ChannelFactory<IHelloWorld>(binding, endpointAddress);
            serviceProxy2 = factory2.CreateChannel();
        }

        // *** EXECUTE Variation *** \\
        try
        {
            string testStr = "test string";
            var intParam = new IntParams() { p1 = 5, p2 = 10 };
            var floatParam = new FloatParams() { p1 = 5.0f, p2 = 10.0f };
            var byteParam = new ByteParams() { p1 = 5, p2 = 10 };

            Assert.Equal(3, serviceProxy1.Sum2(1, 2));
            Assert.Equal(intParam.p1 + intParam.p2, serviceProxy1.Sum(intParam));
            Assert.Equal(string.Format("{0}{1}", intParam.p1, intParam.p2), serviceProxy1.Concatenate(intParam));
            Assert.Equal((float)(floatParam.p1 / floatParam.p2), serviceProxy1.Divide(floatParam));
            Assert.Equal((new byte[] { byteParam.p1, byteParam.p2 }), serviceProxy1.CreateSet(byteParam));
            Assert.Equal(DateTime.Now.Date, serviceProxy1.GetCurrentDateTime().Date);
            
            serviceProxy1.SetIntParamsProperty(intParam);
            Assert.Equal(intParam, serviceProxy1.GetIntParamsProperty());
            
            if (isMultiNs)
            {
                serviceProxy2.SetStringProperty(testStr);
                Assert.Equal(testStr, serviceProxy2.GetStringProperty());
            }
        }
        catch (Exception ex)
        {
            Assert.True(false, ex.Message);
        }
        finally
        {
            // *** ENSURE CLEANUP *** \\
            ScenarioTestHelpers.CloseCommunicationObjects((ICommunicationObject)serviceProxy1);
            if (isMultiNs)
            {
                ScenarioTestHelpers.CloseCommunicationObjects((ICommunicationObject)serviceProxy2);
            }
        }
    } 
}
