// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.Serialization;
using System.ServiceModel.Diagnostics;
using System.ServiceModel.Dispatcher;
using System.Threading;
using System.Xml;

namespace System.ServiceModel.Channels
{
    public sealed class MessageHeaders : IEnumerable<MessageHeaderInfo>
    {
        private int _collectionVersion;
        private int _headerCount;
        private Header[] _headers;
        private MessageVersion _version;
        private IBufferedMessageData _bufferedMessageData;
        private UnderstoodHeaders _understoodHeaders;
        private const int InitialHeaderCount = 4;
        private const int MaxRecycledArrayLength = 8;
        private static XmlDictionaryString[] s_localNames;

        internal const string WildcardAction = "*";

        // The highest node and attribute counts reached by the BVTs were 1829 and 667 respectively.
        private const int MaxBufferedHeaderNodes = 4096;
        private const int MaxBufferedHeaderAttributes = 2048;
        private int _nodeCount = 0;
        private int _attrCount = 0;
        private bool _understoodHeadersModified;

        public MessageHeaders(MessageVersion version, int initialSize)
        {
            Init(version, initialSize);
        }

        public MessageHeaders(MessageVersion version)
            : this(version, InitialHeaderCount)
        {
        }

        internal MessageHeaders(MessageVersion version, XmlDictionaryReader reader, XmlAttributeHolder[] envelopeAttributes, XmlAttributeHolder[] headerAttributes, ref int maxSizeOfHeaders)
            : this(version)
        {
            if (maxSizeOfHeaders < 0)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                    new ArgumentOutOfRangeException("maxSizeOfHeaders", maxSizeOfHeaders,
                    SRServiceModel.ValueMustBeNonNegative));
            }

            if (version == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("version"));
            if (reader == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("reader"));
            if (reader.IsEmptyElement)
            {
                reader.Read();
                return;
            }
            XmlBuffer xmlBuffer = null;
            EnvelopeVersion envelopeVersion = version.Envelope;
            reader.ReadStartElement(XD.MessageDictionary.Header, envelopeVersion.DictionaryNamespace);
            while (reader.IsStartElement())
            {
                if (xmlBuffer == null)
                    xmlBuffer = new XmlBuffer(maxSizeOfHeaders);
                BufferedHeader bufferedHeader = new BufferedHeader(version, xmlBuffer, reader, envelopeAttributes, headerAttributes);
                HeaderProcessing processing = bufferedHeader.MustUnderstand ? HeaderProcessing.MustUnderstand : 0;
                HeaderKind kind = GetHeaderKind(bufferedHeader);
                if (kind != HeaderKind.Unknown)
                {
                    processing |= HeaderProcessing.Understood;
                    MessageHeaders.TraceUnderstood(bufferedHeader);
                }
                Header newHeader = new Header(kind, bufferedHeader, processing);
                AddHeader(newHeader);
            }
            if (xmlBuffer != null)
            {
                xmlBuffer.Close();
                maxSizeOfHeaders -= xmlBuffer.BufferSize;
            }
            reader.ReadEndElement();
            _collectionVersion = 0;
        }

        internal MessageHeaders(MessageVersion version, XmlDictionaryReader reader, IBufferedMessageData bufferedMessageData, RecycledMessageState recycledMessageState, bool[] understoodHeaders, bool understoodHeadersModified)
        {
            _headers = new Header[InitialHeaderCount];
            Init(version, reader, bufferedMessageData, recycledMessageState, understoodHeaders, understoodHeadersModified);
        }

        internal MessageHeaders(MessageVersion version, MessageHeaders headers, IBufferedMessageData bufferedMessageData)
        {
            _version = version;
            _bufferedMessageData = bufferedMessageData;
            _headerCount = headers._headerCount;
            _headers = new Header[_headerCount];
            Array.Copy(headers._headers, _headers, _headerCount);
            _collectionVersion = 0;
        }

        public MessageHeaders(MessageHeaders collection)
        {
            if (collection == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("collection");

            Init(collection._version, collection._headers.Length);
            CopyHeadersFrom(collection);
            _collectionVersion = 0;
        }

        public string Action
        {
            get
            {
                int index = FindHeaderProperty(HeaderKind.Action);
                if (index < 0)
                    return null;
                ActionHeader actionHeader = _headers[index].HeaderInfo as ActionHeader;
                if (actionHeader != null)
                    return actionHeader.Action;
                using (XmlDictionaryReader reader = GetReaderAtHeader(index))
                {
                    return ActionHeader.ReadHeaderValue(reader, _version.Addressing);
                }
            }
            set
            {
                if (value != null)
                    SetActionHeader(ActionHeader.Create(value, _version.Addressing));
                else
                    SetHeaderProperty(HeaderKind.Action, null);
            }
        }

        internal bool CanRecycle
        {
            get { return _headers.Length <= MaxRecycledArrayLength; }
        }

        internal bool ContainsOnlyBufferedMessageHeaders
        {
            get { return (_bufferedMessageData != null && _collectionVersion == 0); }
        }

        internal int CollectionVersion
        {
            get { return _collectionVersion; }
        }

        public int Count
        {
            get { return _headerCount; }
        }

        public EndpointAddress FaultTo
        {
            get
            {
                int index = FindHeaderProperty(HeaderKind.FaultTo);
                if (index < 0)
                    return null;
                FaultToHeader faultToHeader = _headers[index].HeaderInfo as FaultToHeader;
                if (faultToHeader != null)
                    return faultToHeader.FaultTo;
                using (XmlDictionaryReader reader = GetReaderAtHeader(index))
                {
                    return FaultToHeader.ReadHeaderValue(reader, _version.Addressing);
                }
            }
            set
            {
                if (value != null)
                    SetFaultToHeader(FaultToHeader.Create(value, _version.Addressing));
                else
                    SetHeaderProperty(HeaderKind.FaultTo, null);
            }
        }

        public EndpointAddress From
        {
            get
            {
                int index = FindHeaderProperty(HeaderKind.From);
                if (index < 0)
                    return null;
                FromHeader fromHeader = _headers[index].HeaderInfo as FromHeader;
                if (fromHeader != null)
                    return fromHeader.From;
                using (XmlDictionaryReader reader = GetReaderAtHeader(index))
                {
                    return FromHeader.ReadHeaderValue(reader, _version.Addressing);
                }
            }
            set
            {
                if (value != null)
                    SetFromHeader(FromHeader.Create(value, _version.Addressing));
                else
                    SetHeaderProperty(HeaderKind.From, null);
            }
        }

        internal bool HasMustUnderstandBeenModified
        {
            get
            {
                if (_understoodHeaders != null)
                {
                    return _understoodHeaders.Modified;
                }
                else
                {
                    return _understoodHeadersModified;
                }
            }
        }

        public UniqueId MessageId
        {
            get
            {
                int index = FindHeaderProperty(HeaderKind.MessageId);
                if (index < 0)
                    return null;
                MessageIDHeader messageIDHeader = _headers[index].HeaderInfo as MessageIDHeader;
                if (messageIDHeader != null)
                    return messageIDHeader.MessageId;
                using (XmlDictionaryReader reader = GetReaderAtHeader(index))
                {
                    return MessageIDHeader.ReadHeaderValue(reader, _version.Addressing);
                }
            }
            set
            {
                if (value != null)
                    SetMessageIDHeader(MessageIDHeader.Create(value, _version.Addressing));
                else
                    SetHeaderProperty(HeaderKind.MessageId, null);
            }
        }

        public MessageVersion MessageVersion
        {
            get { return _version; }
        }

        public UniqueId RelatesTo
        {
            get
            {
                return GetRelatesTo(RelatesToHeader.ReplyRelationshipType);
            }
            set
            {
                SetRelatesTo(RelatesToHeader.ReplyRelationshipType, value);
            }
        }

        public EndpointAddress ReplyTo
        {
            get
            {
                int index = FindHeaderProperty(HeaderKind.ReplyTo);
                if (index < 0)
                    return null;
                ReplyToHeader replyToHeader = _headers[index].HeaderInfo as ReplyToHeader;
                if (replyToHeader != null)
                    return replyToHeader.ReplyTo;
                using (XmlDictionaryReader reader = GetReaderAtHeader(index))
                {
                    return ReplyToHeader.ReadHeaderValue(reader, _version.Addressing);
                }
            }
            set
            {
                if (value != null)
                    SetReplyToHeader(ReplyToHeader.Create(value, _version.Addressing));
                else
                    SetHeaderProperty(HeaderKind.ReplyTo, null);
            }
        }

        public Uri To
        {
            get
            {
                int index = FindHeaderProperty(HeaderKind.To);
                if (index < 0)
                    return null;
                ToHeader toHeader = _headers[index].HeaderInfo as ToHeader;
                if (toHeader != null)
                    return toHeader.To;
                using (XmlDictionaryReader reader = GetReaderAtHeader(index))
                {
                    return ToHeader.ReadHeaderValue(reader, _version.Addressing);
                }
            }
            set
            {
                if (value != null)
                    SetToHeader(ToHeader.Create(value, _version.Addressing));
                else
                    SetHeaderProperty(HeaderKind.To, null);
            }
        }

        public UnderstoodHeaders UnderstoodHeaders
        {
            get
            {
                if (_understoodHeaders == null)
                    _understoodHeaders = new UnderstoodHeaders(this, _understoodHeadersModified);
                return _understoodHeaders;
            }
        }

        public MessageHeaderInfo this[int index]
        {
            get
            {
                if (index < 0 || index >= _headerCount)
                {
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                        new ArgumentOutOfRangeException("index", index,
                        string.Format(SRServiceModel.ValueMustBeInRange, 0, _headerCount)));
                }

                return _headers[index].HeaderInfo;
            }
        }

        public void Add(MessageHeader header)
        {
            Insert(_headerCount, header);
        }

        internal void AddActionHeader(ActionHeader actionHeader)
        {
            Insert(_headerCount, actionHeader, HeaderKind.Action);
        }

        internal void AddMessageIDHeader(MessageIDHeader messageIDHeader)
        {
            Insert(_headerCount, messageIDHeader, HeaderKind.MessageId);
        }

        internal void AddRelatesToHeader(RelatesToHeader relatesToHeader)
        {
            Insert(_headerCount, relatesToHeader, HeaderKind.RelatesTo);
        }

        internal void AddReplyToHeader(ReplyToHeader replyToHeader)
        {
            Insert(_headerCount, replyToHeader, HeaderKind.ReplyTo);
        }

        internal void AddToHeader(ToHeader toHeader)
        {
            Insert(_headerCount, toHeader, HeaderKind.To);
        }

        private void Add(MessageHeader header, HeaderKind kind)
        {
            Insert(_headerCount, header, kind);
        }

        private void AddHeader(Header header)
        {
            InsertHeader(_headerCount, header);
        }

        internal void AddUnderstood(int i)
        {
            _headers[i].HeaderProcessing |= HeaderProcessing.Understood;
            MessageHeaders.TraceUnderstood(_headers[i].HeaderInfo);
        }

        internal void AddUnderstood(MessageHeaderInfo headerInfo)
        {
            if (headerInfo == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("headerInfo"));
            for (int i = 0; i < _headerCount; i++)
            {
                if ((object)_headers[i].HeaderInfo == (object)headerInfo)
                {
                    if ((_headers[i].HeaderProcessing & HeaderProcessing.Understood) != 0)
                    {
                        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(
                            string.Format(SRServiceModel.HeaderAlreadyUnderstood, headerInfo.Name, headerInfo.Namespace), "headerInfo"));
                    }

                    AddUnderstood(i);
                }
            }
        }

        private void CaptureBufferedHeaders()
        {
            CaptureBufferedHeaders(-1);
        }

        private void CaptureBufferedHeaders(int exceptIndex)
        {
            using (XmlDictionaryReader reader = GetBufferedMessageHeaderReaderAtHeaderContents(_bufferedMessageData))
            {
                for (int i = 0; i < _headerCount; i++)
                {
                    if (reader.NodeType != XmlNodeType.Element)
                    {
                        if (reader.MoveToContent() != XmlNodeType.Element)
                            break;
                    }

                    Header header = _headers[i];
                    if (i == exceptIndex || header.HeaderType != HeaderType.BufferedMessageHeader)
                    {
                        reader.Skip();
                    }
                    else
                    {
                        _headers[i] = new Header(header.HeaderKind, CaptureBufferedHeader(reader,
                            header.HeaderInfo), header.HeaderProcessing);
                    }
                }
            }
            _bufferedMessageData = null;
        }

        private BufferedHeader CaptureBufferedHeader(XmlDictionaryReader reader, MessageHeaderInfo headerInfo)
        {
            XmlBuffer buffer = new XmlBuffer(int.MaxValue);
            XmlDictionaryWriter writer = buffer.OpenSection(_bufferedMessageData.Quotas);
            writer.WriteNode(reader, false);
            buffer.CloseSection();
            buffer.Close();
            return new BufferedHeader(_version, buffer, 0, headerInfo);
        }

        private BufferedHeader CaptureBufferedHeader(IBufferedMessageData bufferedMessageData, MessageHeaderInfo headerInfo, int bufferedMessageHeaderIndex)
        {
            XmlBuffer buffer = new XmlBuffer(int.MaxValue);
            XmlDictionaryWriter writer = buffer.OpenSection(bufferedMessageData.Quotas);
            WriteBufferedMessageHeader(bufferedMessageData, bufferedMessageHeaderIndex, writer);
            buffer.CloseSection();
            buffer.Close();
            return new BufferedHeader(_version, buffer, 0, headerInfo);
        }

        private BufferedHeader CaptureWriteableHeader(MessageHeader writeableHeader)
        {
            XmlBuffer buffer = new XmlBuffer(int.MaxValue);
            XmlDictionaryWriter writer = buffer.OpenSection(XmlDictionaryReaderQuotas.Max);
            writeableHeader.WriteHeader(writer, _version);
            buffer.CloseSection();
            buffer.Close();
            return new BufferedHeader(_version, buffer, 0, writeableHeader);
        }

        public void Clear()
        {
            for (int i = 0; i < _headerCount; i++)
                _headers[i] = new Header();
            _headerCount = 0;
            _collectionVersion++;
            _bufferedMessageData = null;
        }

        public void CopyHeaderFrom(Message message, int headerIndex)
        {
            if (message == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("message"));
            CopyHeaderFrom(message.Headers, headerIndex);
        }

        public void CopyHeaderFrom(MessageHeaders collection, int headerIndex)
        {
            if (collection == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("collection");
            }

            if (collection._version != _version)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(string.Format(SRServiceModel.MessageHeaderVersionMismatch, collection._version.ToString(), _version.ToString()), "collection"));
            }

            if (headerIndex < 0 || headerIndex >= collection._headerCount)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                    new ArgumentOutOfRangeException("headerIndex", headerIndex,
                    string.Format(SRServiceModel.ValueMustBeInRange, 0, collection._headerCount)));
            }
            Header header = collection._headers[headerIndex];
            HeaderProcessing processing = header.HeaderInfo.MustUnderstand ? HeaderProcessing.MustUnderstand : 0;
            if ((header.HeaderProcessing & HeaderProcessing.Understood) != 0 || header.HeaderKind != HeaderKind.Unknown)
                processing |= HeaderProcessing.Understood;
            switch (header.HeaderType)
            {
                case HeaderType.BufferedMessageHeader:
                    AddHeader(new Header(header.HeaderKind, collection.CaptureBufferedHeader(collection._bufferedMessageData,
                        header.HeaderInfo, headerIndex), processing));
                    break;
                case HeaderType.ReadableHeader:
                    AddHeader(new Header(header.HeaderKind, header.ReadableHeader, processing));
                    break;
                case HeaderType.WriteableHeader:
                    AddHeader(new Header(header.HeaderKind, header.MessageHeader, processing));
                    break;
                default:
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(string.Format(SRServiceModel.InvalidEnumValue, header.HeaderType)));
            }
        }

        public void CopyHeadersFrom(Message message)
        {
            if (message == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("message"));
            CopyHeadersFrom(message.Headers);
        }

        public void CopyHeadersFrom(MessageHeaders collection)
        {
            if (collection == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("collection"));
            for (int i = 0; i < collection._headerCount; i++)
                CopyHeaderFrom(collection, i);
        }

        public void CopyTo(MessageHeaderInfo[] array, int index)
        {
            if (array == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("array");
            }

            if (index < 0 || (index + _headerCount) > array.Length)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                    new ArgumentOutOfRangeException("index", index,
                    string.Format(SRServiceModel.ValueMustBeInRange, 0, array.Length - _headerCount)));
            }
            for (int i = 0; i < _headerCount; i++)
                array[i + index] = _headers[i].HeaderInfo;
        }

        private Exception CreateDuplicateHeaderException(HeaderKind kind)
        {
            string name;
            switch (kind)
            {
                case HeaderKind.Action:
                    name = AddressingStrings.Action;
                    break;
                case HeaderKind.FaultTo:
                    name = AddressingStrings.FaultTo;
                    break;
                case HeaderKind.From:
                    name = AddressingStrings.From;
                    break;
                case HeaderKind.MessageId:
                    name = AddressingStrings.MessageId;
                    break;
                case HeaderKind.ReplyTo:
                    name = AddressingStrings.ReplyTo;
                    break;
                case HeaderKind.To:
                    name = AddressingStrings.To;
                    break;
                default:
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(string.Format(SRServiceModel.InvalidEnumValue, kind)));
            }

            return new MessageHeaderException(
                string.Format(SRServiceModel.MultipleMessageHeaders, name, _version.Addressing.Namespace),
                name,
                _version.Addressing.Namespace,
                true);
        }

        public int FindHeader(string name, string ns)
        {
            if (name == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("name"));
            if (ns == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("ns"));

            if (ns == _version.Addressing.Namespace)
            {
                return FindAddressingHeader(name, ns);
            }
            else
            {
                return FindNonAddressingHeader(name, ns, _version.Envelope.UltimateDestinationActorValues);
            }
        }

        private int FindAddressingHeader(string name, string ns)
        {
            int foundAt = -1;
            for (int i = 0; i < _headerCount; i++)
            {
                if (_headers[i].HeaderKind != HeaderKind.Unknown)
                {
                    MessageHeaderInfo info = _headers[i].HeaderInfo;
                    if (info.Name == name && info.Namespace == ns)
                    {
                        if (foundAt >= 0)
                        {
                            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                                new MessageHeaderException(string.Format(SRServiceModel.MultipleMessageHeaders, name, ns), name, ns, true));
                        }
                        foundAt = i;
                    }
                }
            }
            return foundAt;
        }

        private int FindNonAddressingHeader(string name, string ns, string[] actors)
        {
            int foundAt = -1;
            for (int i = 0; i < _headerCount; i++)
            {
                if (_headers[i].HeaderKind == HeaderKind.Unknown)
                {
                    MessageHeaderInfo info = _headers[i].HeaderInfo;
                    if (info.Name == name && info.Namespace == ns)
                    {
                        for (int j = 0; j < actors.Length; j++)
                        {
                            if (actors[j] == info.Actor)
                            {
                                if (foundAt >= 0)
                                {
                                    if (actors.Length == 1)
                                        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new MessageHeaderException(string.Format(SRServiceModel.MultipleMessageHeadersWithActor, name, ns, actors[0]), name, ns, true));
                                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new MessageHeaderException(string.Format(SRServiceModel.MultipleMessageHeaders, name, ns), name, ns, true));
                                }
                                foundAt = i;
                            }
                        }
                    }
                }
            }
            return foundAt;
        }

        public int FindHeader(string name, string ns, params string[] actors)
        {
            if (name == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("name"));
            if (ns == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("ns"));
            if (actors == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("actors"));
            int foundAt = -1;
            for (int i = 0; i < _headerCount; i++)
            {
                MessageHeaderInfo info = _headers[i].HeaderInfo;
                if (info.Name == name && info.Namespace == ns)
                {
                    for (int j = 0; j < actors.Length; j++)
                    {
                        if (actors[j] == info.Actor)
                        {
                            if (foundAt >= 0)
                            {
                                if (actors.Length == 1)
                                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new MessageHeaderException(string.Format(SRServiceModel.MultipleMessageHeadersWithActor, name, ns, actors[0]), name, ns, true));
                                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new MessageHeaderException(string.Format(SRServiceModel.MultipleMessageHeaders, name, ns), name, ns, true));
                            }
                            foundAt = i;
                        }
                    }
                }
            }
            return foundAt;
        }

        private int FindHeaderProperty(HeaderKind kind)
        {
            int index = -1;
            for (int i = 0; i < _headerCount; i++)
            {
                if (_headers[i].HeaderKind == kind)
                {
                    if (index >= 0)
                        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(CreateDuplicateHeaderException(kind));
                    index = i;
                }
            }
            return index;
        }

        private int FindRelatesTo(Uri relationshipType, out UniqueId messageId)
        {
            UniqueId foundValue = null;
            int foundIndex = -1;
            for (int i = 0; i < _headerCount; i++)
            {
                if (_headers[i].HeaderKind == HeaderKind.RelatesTo)
                {
                    Uri tempRelationship;
                    UniqueId tempValue;
                    GetRelatesToValues(i, out tempRelationship, out tempValue);

                    if (relationshipType == tempRelationship)
                    {
                        if (foundValue != null)
                        {
                            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                                new MessageHeaderException(
                                    string.Format(SRServiceModel.MultipleRelatesToHeaders, relationshipType.AbsoluteUri),
                                    AddressingStrings.RelatesTo,
                                    _version.Addressing.Namespace,
                                    true));
                        }
                        foundValue = tempValue;
                        foundIndex = i;
                    }
                }
            }

            messageId = foundValue;
            return foundIndex;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<MessageHeaderInfo> GetEnumerator()
        {
            MessageHeaderInfo[] headers = new MessageHeaderInfo[_headerCount];
            CopyTo(headers, 0);
            return GetEnumerator(headers);
        }

        private IEnumerator<MessageHeaderInfo> GetEnumerator(MessageHeaderInfo[] headers)
        {
            IList<MessageHeaderInfo> list = new ReadOnlyCollection<MessageHeaderInfo>(headers);
            return list.GetEnumerator();
        }

        internal IEnumerator<MessageHeaderInfo> GetUnderstoodEnumerator()
        {
            List<MessageHeaderInfo> understoodHeaders = new List<MessageHeaderInfo>();

            for (int i = 0; i < _headerCount; i++)
            {
                if ((_headers[i].HeaderProcessing & HeaderProcessing.Understood) != 0)
                {
                    understoodHeaders.Add(_headers[i].HeaderInfo);
                }
            }

            return understoodHeaders.GetEnumerator();
        }

        private static XmlDictionaryReader GetBufferedMessageHeaderReaderAtHeaderContents(IBufferedMessageData bufferedMessageData)
        {
            XmlDictionaryReader reader = bufferedMessageData.GetMessageReader();
            if (reader.NodeType == XmlNodeType.Element)
                reader.Read();
            else
                reader.ReadStartElement();
            if (reader.NodeType == XmlNodeType.Element)
                reader.Read();
            else
                reader.ReadStartElement();
            return reader;
        }

        private XmlDictionaryReader GetBufferedMessageHeaderReader(IBufferedMessageData bufferedMessageData, int bufferedMessageHeaderIndex)
        {
            // Check if we need to change representations
            if (_nodeCount > MaxBufferedHeaderNodes || _attrCount > MaxBufferedHeaderAttributes)
            {
                CaptureBufferedHeaders();
                return _headers[bufferedMessageHeaderIndex].ReadableHeader.GetHeaderReader();
            }

            XmlDictionaryReader reader = GetBufferedMessageHeaderReaderAtHeaderContents(bufferedMessageData);
            for (; ; )
            {
                if (reader.NodeType != XmlNodeType.Element)
                    reader.MoveToContent();
                if (bufferedMessageHeaderIndex == 0)
                    break;
                Skip(reader);
                bufferedMessageHeaderIndex--;
            }

            return reader;
        }

        private void Skip(XmlDictionaryReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element && !reader.IsEmptyElement)
            {
                int depth = reader.Depth;
                do
                {
                    _attrCount += reader.AttributeCount;
                    _nodeCount++;
                } while (reader.Read() && depth < reader.Depth);

                // consume end tag
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    _nodeCount++;
                    reader.Read();
                }
            }
            else
            {
                _attrCount += reader.AttributeCount;
                _nodeCount++;
                reader.Read();
            }
        }

        public T GetHeader<T>(string name, string ns)
        {
            return GetHeader<T>(name, ns, DataContractSerializerDefaults.CreateSerializer(typeof(T), name, ns, int.MaxValue/*maxItems*/));
        }

        public T GetHeader<T>(string name, string ns, params string[] actors)
        {
            int index = FindHeader(name, ns, actors);
            if (index < 0)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new MessageHeaderException(string.Format(SRServiceModel.HeaderNotFound, name, ns), name, ns));
            return GetHeader<T>(index);
        }

        public T GetHeader<T>(string name, string ns, XmlObjectSerializer serializer)
        {
            if (serializer == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("serializer"));
            int index = FindHeader(name, ns);
            if (index < 0)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new MessageHeaderException(string.Format(SRServiceModel.HeaderNotFound, name, ns), name, ns));
            return GetHeader<T>(index, serializer);
        }

        public T GetHeader<T>(int index)
        {
            if (index < 0 || index >= _headerCount)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                    new ArgumentOutOfRangeException("index", index,
                    string.Format(SRServiceModel.ValueMustBeInRange, 0, _headerCount)));
            }

            MessageHeaderInfo headerInfo = _headers[index].HeaderInfo;
            return GetHeader<T>(index, DataContractSerializerDefaults.CreateSerializer(typeof(T), headerInfo.Name, headerInfo.Namespace, int.MaxValue/*maxItems*/));
        }

        public T GetHeader<T>(int index, XmlObjectSerializer serializer)
        {
            if (serializer == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("serializer"));
            using (XmlDictionaryReader reader = GetReaderAtHeader(index))
            {
                return (T)serializer.ReadObject(reader);
            }
        }

        private HeaderKind GetHeaderKind(MessageHeaderInfo headerInfo)
        {
            HeaderKind headerKind = HeaderKind.Unknown;

            if (headerInfo.Namespace == _version.Addressing.Namespace)
            {
                if (_version.Envelope.IsUltimateDestinationActor(headerInfo.Actor))
                {
                    string name = headerInfo.Name;
                    if (name.Length > 0)
                    {
                        switch (name[0])
                        {
                            case 'A':
                                if (name == AddressingStrings.Action)
                                {
                                    headerKind = HeaderKind.Action;
                                }
                                break;
                            case 'F':
                                if (name == AddressingStrings.From)
                                {
                                    headerKind = HeaderKind.From;
                                }
                                else if (name == AddressingStrings.FaultTo)
                                {
                                    headerKind = HeaderKind.FaultTo;
                                }
                                break;
                            case 'M':
                                if (name == AddressingStrings.MessageId)
                                {
                                    headerKind = HeaderKind.MessageId;
                                }
                                break;
                            case 'R':
                                if (name == AddressingStrings.ReplyTo)
                                {
                                    headerKind = HeaderKind.ReplyTo;
                                }
                                else if (name == AddressingStrings.RelatesTo)
                                {
                                    headerKind = HeaderKind.RelatesTo;
                                }
                                break;
                            case 'T':
                                if (name == AddressingStrings.To)
                                {
                                    headerKind = HeaderKind.To;
                                }
                                break;
                        }
                    }
                }
            }

            ValidateHeaderKind(headerKind);
            return headerKind;
        }

        private void ValidateHeaderKind(HeaderKind headerKind)
        {
            if (_version.Envelope == EnvelopeVersion.None)
            {
                if (headerKind != HeaderKind.Action && headerKind != HeaderKind.To)
                {
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                        new InvalidOperationException(string.Format(SRServiceModel.HeadersCannotBeAddedToEnvelopeVersion, _version.Envelope)));
                }
            }

            if (_version.Addressing == AddressingVersion.None)
            {
                if (headerKind != HeaderKind.Unknown && headerKind != HeaderKind.Action && headerKind != HeaderKind.To)
                {
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                        new InvalidOperationException(string.Format(SRServiceModel.AddressingHeadersCannotBeAddedToAddressingVersion, _version.Addressing)));
                }
            }
        }

        public XmlDictionaryReader GetReaderAtHeader(int headerIndex)
        {
            if (headerIndex < 0 || headerIndex >= _headerCount)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                    new ArgumentOutOfRangeException("headerIndex", headerIndex,
                    string.Format(SRServiceModel.ValueMustBeInRange, 0, _headerCount)));
            }

            switch (_headers[headerIndex].HeaderType)
            {
                case HeaderType.ReadableHeader:
                    return _headers[headerIndex].ReadableHeader.GetHeaderReader();
                case HeaderType.WriteableHeader:
                    MessageHeader writeableHeader = _headers[headerIndex].MessageHeader;
                    BufferedHeader bufferedHeader = CaptureWriteableHeader(writeableHeader);
                    _headers[headerIndex] = new Header(_headers[headerIndex].HeaderKind, bufferedHeader, _headers[headerIndex].HeaderProcessing);
                    _collectionVersion++;
                    return bufferedHeader.GetHeaderReader();
                case HeaderType.BufferedMessageHeader:
                    return GetBufferedMessageHeaderReader(_bufferedMessageData, headerIndex);
                default:
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(string.Format(SRServiceModel.InvalidEnumValue, _headers[headerIndex].HeaderType)));
            }
        }

        internal UniqueId GetRelatesTo(Uri relationshipType)
        {
            if (relationshipType == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("relationshipType"));

            UniqueId messageId;
            FindRelatesTo(relationshipType, out messageId);
            return messageId;
        }

        private void GetRelatesToValues(int index, out Uri relationshipType, out UniqueId messageId)
        {
            RelatesToHeader relatesToHeader = _headers[index].HeaderInfo as RelatesToHeader;
            if (relatesToHeader != null)
            {
                relationshipType = relatesToHeader.RelationshipType;
                messageId = relatesToHeader.UniqueId;
            }
            else
            {
                using (XmlDictionaryReader reader = GetReaderAtHeader(index))
                {
                    RelatesToHeader.ReadHeaderValue(reader, _version.Addressing, out relationshipType, out messageId);
                }
            }
        }

        internal string[] GetHeaderAttributes(string localName, string ns)
        {
            string[] attrs = null;

            if (ContainsOnlyBufferedMessageHeaders)
            {
                XmlDictionaryReader reader = _bufferedMessageData.GetMessageReader();
                reader.ReadStartElement(); // Envelope
                reader.ReadStartElement(); // Header
                for (int index = 0; reader.IsStartElement(); index++)
                {
                    string value = reader.GetAttribute(localName, ns);
                    if (value != null)
                    {
                        if (attrs == null)
                            attrs = new string[_headerCount];
                        attrs[index] = value;
                    }
                    if (index == _headerCount - 1)
                        break;
                    reader.Skip();
                }
                reader.Dispose();
            }
            else
            {
                for (int index = 0; index < _headerCount; index++)
                {
                    if (_headers[index].HeaderType != HeaderType.WriteableHeader)
                    {
                        using (XmlDictionaryReader reader = GetReaderAtHeader(index))
                        {
                            string value = reader.GetAttribute(localName, ns);
                            if (value != null)
                            {
                                if (attrs == null)
                                    attrs = new string[_headerCount];
                                attrs[index] = value;
                            }
                        }
                    }
                }
            }

            return attrs;
        }

        internal MessageHeader GetMessageHeader(int index)
        {
            if (index < 0 || index >= _headerCount)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                    new ArgumentOutOfRangeException("headerIndex", index,
                    string.Format(SRServiceModel.ValueMustBeInRange, 0, _headerCount)));
            }
            MessageHeader messageHeader;
            switch (_headers[index].HeaderType)
            {
                case HeaderType.WriteableHeader:
                case HeaderType.ReadableHeader:
                    return _headers[index].MessageHeader;
                case HeaderType.BufferedMessageHeader:
                    messageHeader = CaptureBufferedHeader(_bufferedMessageData, _headers[index].HeaderInfo, index);
                    _headers[index] = new Header(_headers[index].HeaderKind, messageHeader, _headers[index].HeaderProcessing);
                    _collectionVersion++;
                    return messageHeader;
                default:
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(string.Format(SRServiceModel.InvalidEnumValue, _headers[index].HeaderType)));
            }
        }

        internal Collection<MessageHeaderInfo> GetHeadersNotUnderstood()
        {
            Collection<MessageHeaderInfo> notUnderstoodHeaders = null;

            for (int headerIndex = 0; headerIndex < _headerCount; headerIndex++)
            {
                if (_headers[headerIndex].HeaderProcessing == HeaderProcessing.MustUnderstand)
                {
                    if (notUnderstoodHeaders == null)
                        notUnderstoodHeaders = new Collection<MessageHeaderInfo>();

                    MessageHeaderInfo headerInfo = _headers[headerIndex].HeaderInfo;
                    notUnderstoodHeaders.Add(headerInfo);
                }
            }

            return notUnderstoodHeaders;
        }

        public bool HaveMandatoryHeadersBeenUnderstood()
        {
            return HaveMandatoryHeadersBeenUnderstood(_version.Envelope.MustUnderstandActorValues);
        }

        public bool HaveMandatoryHeadersBeenUnderstood(params string[] actors)
        {
            if (actors == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("actors"));

            for (int headerIndex = 0; headerIndex < _headerCount; headerIndex++)
            {
                if (_headers[headerIndex].HeaderProcessing == HeaderProcessing.MustUnderstand)
                {
                    for (int actorIndex = 0; actorIndex < actors.Length; ++actorIndex)
                    {
                        if (_headers[headerIndex].HeaderInfo.Actor == actors[actorIndex])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        internal void Init(MessageVersion version, int initialSize)
        {
            _nodeCount = 0;
            _attrCount = 0;
            if (initialSize < 0)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                    new ArgumentOutOfRangeException("initialSize", initialSize,
                    SRServiceModel.ValueMustBeNonNegative));
            }

            if (version == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("version");
            }

            _version = version;
            _headers = new Header[initialSize];
        }

        internal void Init(MessageVersion version)
        {
            _nodeCount = 0;
            _attrCount = 0;
            _version = version;
            _collectionVersion = 0;
        }

        internal void Init(MessageVersion version, XmlDictionaryReader reader, IBufferedMessageData bufferedMessageData, RecycledMessageState recycledMessageState, bool[] understoodHeaders, bool understoodHeadersModified)
        {
            _nodeCount = 0;
            _attrCount = 0;
            _version = version;
            _bufferedMessageData = bufferedMessageData;

            if (version.Envelope != EnvelopeVersion.None)
            {
                _understoodHeadersModified = (understoodHeaders != null) && understoodHeadersModified;
                if (reader.IsEmptyElement)
                {
                    reader.Read();
                    return;
                }
                EnvelopeVersion envelopeVersion = version.Envelope;
                Fx.Assert(reader.IsStartElement(XD.MessageDictionary.Header, envelopeVersion.DictionaryNamespace), "");
                reader.ReadStartElement();

                AddressingDictionary dictionary = XD.AddressingDictionary;

                if (s_localNames == null)
                {
                    XmlDictionaryString[] strings = new XmlDictionaryString[7];
                    strings[(int)HeaderKind.To] = dictionary.To;
                    strings[(int)HeaderKind.Action] = dictionary.Action;
                    strings[(int)HeaderKind.MessageId] = dictionary.MessageId;
                    strings[(int)HeaderKind.RelatesTo] = dictionary.RelatesTo;
                    strings[(int)HeaderKind.ReplyTo] = dictionary.ReplyTo;
                    strings[(int)HeaderKind.From] = dictionary.From;
                    strings[(int)HeaderKind.FaultTo] = dictionary.FaultTo;
                    Interlocked.MemoryBarrier();
                    s_localNames = strings;
                }


                int i = 0;
                while (reader.IsStartElement())
                {
                    ReadBufferedHeader(reader, recycledMessageState, s_localNames, (understoodHeaders == null) ? false : understoodHeaders[i++]);
                }

                reader.ReadEndElement();
            }
            _collectionVersion = 0;
        }

        public void Insert(int headerIndex, MessageHeader header)
        {
            if (header == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("header"));
            if (!header.IsMessageVersionSupported(_version))
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(string.Format(SRServiceModel.MessageHeaderVersionNotSupported,
                    header.GetType().FullName, _version.Envelope.ToString()), "header"));
            Insert(headerIndex, header, GetHeaderKind(header));
        }

        private void Insert(int headerIndex, MessageHeader header, HeaderKind kind)
        {
            ReadableMessageHeader readableMessageHeader = header as ReadableMessageHeader;
            HeaderProcessing processing = header.MustUnderstand ? HeaderProcessing.MustUnderstand : 0;
            if (kind != HeaderKind.Unknown)
                processing |= HeaderProcessing.Understood;
            if (readableMessageHeader != null)
                InsertHeader(headerIndex, new Header(kind, readableMessageHeader, processing));
            else
                InsertHeader(headerIndex, new Header(kind, header, processing));
        }

        private void InsertHeader(int headerIndex, Header header)
        {
            ValidateHeaderKind(header.HeaderKind);

            if (headerIndex < 0 || headerIndex > _headerCount)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                    new ArgumentOutOfRangeException("headerIndex", headerIndex,
                    string.Format(SRServiceModel.ValueMustBeInRange, 0, _headerCount)));
            }

            if (_headerCount == _headers.Length)
            {
                if (_headers.Length == 0)
                {
                    _headers = new Header[1];
                }
                else
                {
                    Header[] newHeaders = new Header[_headers.Length * 2];
                    _headers.CopyTo(newHeaders, 0);
                    _headers = newHeaders;
                }
            }
            if (headerIndex < _headerCount)
            {
                if (_bufferedMessageData != null)
                {
                    for (int i = headerIndex; i < _headerCount; i++)
                    {
                        if (_headers[i].HeaderType == HeaderType.BufferedMessageHeader)
                        {
                            CaptureBufferedHeaders();
                            break;
                        }
                    }
                }
                Array.Copy(_headers, headerIndex, _headers, headerIndex + 1, _headerCount - headerIndex);
            }
            _headers[headerIndex] = header;
            _headerCount++;
            _collectionVersion++;
        }

        internal bool IsUnderstood(int i)
        {
            return (_headers[i].HeaderProcessing & HeaderProcessing.Understood) != 0;
        }

        internal bool IsUnderstood(MessageHeaderInfo headerInfo)
        {
            if (headerInfo == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("headerInfo"));
            for (int i = 0; i < _headerCount; i++)
            {
                if ((object)_headers[i].HeaderInfo == (object)headerInfo)
                {
                    if (IsUnderstood(i))
                        return true;
                }
            }

            return false;
        }

        private void ReadBufferedHeader(XmlDictionaryReader reader, RecycledMessageState recycledMessageState, XmlDictionaryString[] localNames, bool understood)
        {
            string actor;
            bool mustUnderstand;
            bool relay;
            bool isRefParam;

            if (_version.Addressing == AddressingVersion.None && reader.NamespaceURI == AddressingVersion.None.Namespace)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                    new InvalidOperationException(string.Format(SRServiceModel.AddressingHeadersCannotBeAddedToAddressingVersion, _version.Addressing)));
            }

            MessageHeader.GetHeaderAttributes(reader, _version, out actor, out mustUnderstand, out relay, out isRefParam);

            HeaderKind kind = HeaderKind.Unknown;
            MessageHeaderInfo info = null;

            if (_version.Envelope.IsUltimateDestinationActor(actor))
            {
                Fx.Assert(_version.Addressing.DictionaryNamespace != null, "non-None Addressing requires a non-null DictionaryNamespace");
                kind = (HeaderKind)reader.IndexOfLocalName(localNames, _version.Addressing.DictionaryNamespace);
                switch (kind)
                {
                    case HeaderKind.To:
                        info = ToHeader.ReadHeader(reader, _version.Addressing, recycledMessageState.UriCache, actor, mustUnderstand, relay);
                        break;
                    case HeaderKind.Action:
                        info = ActionHeader.ReadHeader(reader, _version.Addressing, actor, mustUnderstand, relay);
                        break;
                    case HeaderKind.MessageId:
                        info = MessageIDHeader.ReadHeader(reader, _version.Addressing, actor, mustUnderstand, relay);
                        break;
                    case HeaderKind.RelatesTo:
                        info = RelatesToHeader.ReadHeader(reader, _version.Addressing, actor, mustUnderstand, relay);
                        break;
                    case HeaderKind.ReplyTo:
                        info = ReplyToHeader.ReadHeader(reader, _version.Addressing, actor, mustUnderstand, relay);
                        break;
                    case HeaderKind.From:
                        info = FromHeader.ReadHeader(reader, _version.Addressing, actor, mustUnderstand, relay);
                        break;
                    case HeaderKind.FaultTo:
                        info = FaultToHeader.ReadHeader(reader, _version.Addressing, actor, mustUnderstand, relay);
                        break;
                    default:
                        kind = HeaderKind.Unknown;
                        break;
                }
            }

            if (info == null)
            {
                info = recycledMessageState.HeaderInfoCache.TakeHeaderInfo(reader, actor, mustUnderstand, relay, isRefParam);
                reader.Skip();
            }

            HeaderProcessing processing = mustUnderstand ? HeaderProcessing.MustUnderstand : 0;
            if (kind != HeaderKind.Unknown || understood)
            {
                processing |= HeaderProcessing.Understood;
                MessageHeaders.TraceUnderstood(info);
            }
            AddHeader(new Header(kind, info, processing));
        }

        internal void Recycle(HeaderInfoCache headerInfoCache)
        {
            for (int i = 0; i < _headerCount; i++)
            {
                if (_headers[i].HeaderKind == HeaderKind.Unknown)
                {
                    headerInfoCache.ReturnHeaderInfo(_headers[i].HeaderInfo);
                }
            }
            Clear();
            _collectionVersion = 0;
            if (_understoodHeaders != null)
            {
                _understoodHeaders.Modified = false;
            }
        }

        internal void RemoveUnderstood(MessageHeaderInfo headerInfo)
        {
            if (headerInfo == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("headerInfo"));
            for (int i = 0; i < _headerCount; i++)
            {
                if ((object)_headers[i].HeaderInfo == (object)headerInfo)
                {
                    if ((_headers[i].HeaderProcessing & HeaderProcessing.Understood) == 0)
                    {
                        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(
                            string.Format(SRServiceModel.HeaderAlreadyNotUnderstood, headerInfo.Name, headerInfo.Namespace), "headerInfo"));
                    }

                    _headers[i].HeaderProcessing &= ~HeaderProcessing.Understood;
                }
            }
        }

        public void RemoveAll(string name, string ns)
        {
            if (name == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("name"));
            if (ns == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("ns"));
            for (int i = _headerCount - 1; i >= 0; i--)
            {
                MessageHeaderInfo info = _headers[i].HeaderInfo;
                if (info.Name == name && info.Namespace == ns)
                {
                    RemoveAt(i);
                }
            }
        }

        public void RemoveAt(int headerIndex)
        {
            if (headerIndex < 0 || headerIndex >= _headerCount)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                    new ArgumentOutOfRangeException("headerIndex", headerIndex,
                    string.Format(SRServiceModel.ValueMustBeInRange, 0, _headerCount)));
            }
            if (_bufferedMessageData != null && _headers[headerIndex].HeaderType == HeaderType.BufferedMessageHeader)
                CaptureBufferedHeaders(headerIndex);
            Array.Copy(_headers, headerIndex + 1, _headers, headerIndex, _headerCount - headerIndex - 1);
            _headers[--_headerCount] = new Header();
            _collectionVersion++;
        }

        internal void ReplaceAt(int headerIndex, MessageHeader header)
        {
            if (headerIndex < 0 || headerIndex >= _headerCount)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                    new ArgumentOutOfRangeException("headerIndex", headerIndex,
                    string.Format(SRServiceModel.ValueMustBeInRange, 0, _headerCount)));
            }

            if (header == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("header");
            }

            ReplaceAt(headerIndex, header, GetHeaderKind(header));
        }

        private void ReplaceAt(int headerIndex, MessageHeader header, HeaderKind kind)
        {
            HeaderProcessing processing = header.MustUnderstand ? HeaderProcessing.MustUnderstand : 0;
            if (kind != HeaderKind.Unknown)
                processing |= HeaderProcessing.Understood;
            ReadableMessageHeader readableMessageHeader = header as ReadableMessageHeader;
            if (readableMessageHeader != null)
                _headers[headerIndex] = new Header(kind, readableMessageHeader, processing);
            else
                _headers[headerIndex] = new Header(kind, header, processing);
            _collectionVersion++;
        }

        public void SetAction(XmlDictionaryString action)
        {
            if (action == null)
                SetHeaderProperty(HeaderKind.Action, null);
            else
                SetActionHeader(ActionHeader.Create(action, _version.Addressing));
        }

        internal void SetActionHeader(ActionHeader actionHeader)
        {
            SetHeaderProperty(HeaderKind.Action, actionHeader);
        }

        internal void SetFaultToHeader(FaultToHeader faultToHeader)
        {
            SetHeaderProperty(HeaderKind.FaultTo, faultToHeader);
        }

        internal void SetFromHeader(FromHeader fromHeader)
        {
            SetHeaderProperty(HeaderKind.From, fromHeader);
        }

        internal void SetMessageIDHeader(MessageIDHeader messageIDHeader)
        {
            SetHeaderProperty(HeaderKind.MessageId, messageIDHeader);
        }

        internal void SetRelatesTo(Uri relationshipType, UniqueId messageId)
        {
            if (relationshipType == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("relationshipType");
            }

            RelatesToHeader relatesToHeader;
            if (!object.ReferenceEquals(messageId, null))
            {
                relatesToHeader = RelatesToHeader.Create(messageId, _version.Addressing, relationshipType);
            }
            else
            {
                relatesToHeader = null;
            }

            SetRelatesTo(RelatesToHeader.ReplyRelationshipType, relatesToHeader);
        }

        private void SetRelatesTo(Uri relationshipType, RelatesToHeader relatesToHeader)
        {
            UniqueId previousUniqueId;
            int index = FindRelatesTo(relationshipType, out previousUniqueId);
            if (index >= 0)
            {
                if (relatesToHeader == null)
                {
                    RemoveAt(index);
                }
                else
                {
                    ReplaceAt(index, relatesToHeader, HeaderKind.RelatesTo);
                }
            }
            else if (relatesToHeader != null)
            {
                Add(relatesToHeader, HeaderKind.RelatesTo);
            }
        }

        internal void SetReplyToHeader(ReplyToHeader replyToHeader)
        {
            SetHeaderProperty(HeaderKind.ReplyTo, replyToHeader);
        }

        internal void SetToHeader(ToHeader toHeader)
        {
            SetHeaderProperty(HeaderKind.To, toHeader);
        }

        private void SetHeaderProperty(HeaderKind kind, MessageHeader header)
        {
            int index = FindHeaderProperty(kind);
            if (index >= 0)
            {
                if (header == null)
                {
                    RemoveAt(index);
                }
                else
                {
                    ReplaceAt(index, header, kind);
                }
            }
            else if (header != null)
            {
                Add(header, kind);
            }
        }

        public void WriteHeader(int headerIndex, XmlWriter writer)
        {
            WriteHeader(headerIndex, XmlDictionaryWriter.CreateDictionaryWriter(writer));
        }

        public void WriteHeader(int headerIndex, XmlDictionaryWriter writer)
        {
            WriteStartHeader(headerIndex, writer);
            WriteHeaderContents(headerIndex, writer);
            writer.WriteEndElement();
        }

        public void WriteStartHeader(int headerIndex, XmlWriter writer)
        {
            WriteStartHeader(headerIndex, XmlDictionaryWriter.CreateDictionaryWriter(writer));
        }

        public void WriteStartHeader(int headerIndex, XmlDictionaryWriter writer)
        {
            if (writer == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("writer");
            }

            if (headerIndex < 0 || headerIndex >= _headerCount)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                    new ArgumentOutOfRangeException("headerIndex", headerIndex,
                    string.Format(SRServiceModel.ValueMustBeInRange, 0, _headerCount)));
            }
            switch (_headers[headerIndex].HeaderType)
            {
                case HeaderType.ReadableHeader:
                case HeaderType.WriteableHeader:
                    _headers[headerIndex].MessageHeader.WriteStartHeader(writer, _version);
                    break;
                case HeaderType.BufferedMessageHeader:
                    WriteStartBufferedMessageHeader(_bufferedMessageData, headerIndex, writer);
                    break;
                default:
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(string.Format(SRServiceModel.InvalidEnumValue, _headers[headerIndex].HeaderType)));
            }
        }

        public void WriteHeaderContents(int headerIndex, XmlWriter writer)
        {
            WriteHeaderContents(headerIndex, XmlDictionaryWriter.CreateDictionaryWriter(writer));
        }

        public void WriteHeaderContents(int headerIndex, XmlDictionaryWriter writer)
        {
            if (writer == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("writer");
            }

            if (headerIndex < 0 || headerIndex >= _headerCount)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
                    new ArgumentOutOfRangeException("headerIndex", headerIndex,
                    string.Format(SRServiceModel.ValueMustBeInRange, 0, _headerCount)));
            }
            switch (_headers[headerIndex].HeaderType)
            {
                case HeaderType.ReadableHeader:
                case HeaderType.WriteableHeader:
                    _headers[headerIndex].MessageHeader.WriteHeaderContents(writer, _version);
                    break;
                case HeaderType.BufferedMessageHeader:
                    WriteBufferedMessageHeaderContents(_bufferedMessageData, headerIndex, writer);
                    break;
                default:
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(string.Format(SRServiceModel.InvalidEnumValue, _headers[headerIndex].HeaderType)));
            }
        }

        private static void TraceUnderstood(MessageHeaderInfo info)
        {
        }

        private void WriteBufferedMessageHeader(IBufferedMessageData bufferedMessageData, int bufferedMessageHeaderIndex, XmlWriter writer)
        {
            using (XmlReader reader = GetBufferedMessageHeaderReader(bufferedMessageData, bufferedMessageHeaderIndex))
            {
                writer.WriteNode(reader, false);
            }
        }

        private void WriteStartBufferedMessageHeader(IBufferedMessageData bufferedMessageData, int bufferedMessageHeaderIndex, XmlWriter writer)
        {
            using (XmlReader reader = GetBufferedMessageHeaderReader(bufferedMessageData, bufferedMessageHeaderIndex))
            {
                writer.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
                writer.WriteAttributes(reader, false);
            }
        }

        private void WriteBufferedMessageHeaderContents(IBufferedMessageData bufferedMessageData, int bufferedMessageHeaderIndex, XmlWriter writer)
        {
            using (XmlReader reader = GetBufferedMessageHeaderReader(bufferedMessageData, bufferedMessageHeaderIndex))
            {
                if (!reader.IsEmptyElement)
                {
                    reader.ReadStartElement();
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        writer.WriteNode(reader, false);
                    }
                    reader.ReadEndElement();
                }
            }
        }

        internal enum HeaderType : byte
        {
            Invalid,
            ReadableHeader,
            BufferedMessageHeader,
            WriteableHeader
        }

        internal enum HeaderKind : byte
        {
            Action,
            FaultTo,
            From,
            MessageId,
            ReplyTo,
            RelatesTo,
            To,
            Unknown,
        }
        [Flags]

        internal enum HeaderProcessing : byte
        {
            MustUnderstand = 0x1,
            Understood = 0x2,
        }

        internal struct Header
        {
            private HeaderType _type;
            private HeaderKind _kind;
            private HeaderProcessing _processing;
            private MessageHeaderInfo _info;

            public Header(HeaderKind kind, MessageHeaderInfo info, HeaderProcessing processing)
            {
                _kind = kind;
                _type = HeaderType.BufferedMessageHeader;
                _info = info;
                _processing = processing;
            }

            public Header(HeaderKind kind, ReadableMessageHeader readableHeader, HeaderProcessing processing)
            {
                _kind = kind;
                _type = HeaderType.ReadableHeader;
                _info = readableHeader;
                _processing = processing;
            }

            public Header(HeaderKind kind, MessageHeader header, HeaderProcessing processing)
            {
                _kind = kind;
                _type = HeaderType.WriteableHeader;
                _info = header;
                _processing = processing;
            }

            public HeaderType HeaderType
            {
                get { return _type; }
            }

            public HeaderKind HeaderKind
            {
                get { return _kind; }
            }

            public MessageHeaderInfo HeaderInfo
            {
                get { return _info; }
            }

            public MessageHeader MessageHeader
            {
                get
                {
                    Fx.Assert(_type == HeaderType.WriteableHeader || _type == HeaderType.ReadableHeader, "");
                    return (MessageHeader)_info;
                }
            }

            public HeaderProcessing HeaderProcessing
            {
                get { return _processing; }
                set { _processing = value; }
            }

            public ReadableMessageHeader ReadableHeader
            {
                get
                {
                    Fx.Assert(_type == HeaderType.ReadableHeader, "");
                    return (ReadableMessageHeader)_info;
                }
            }
        }
    }
}
