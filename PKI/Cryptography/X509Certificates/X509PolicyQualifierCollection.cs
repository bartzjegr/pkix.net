﻿using System.Collections.Generic;
using System.IO;
using PKI.Base;
using SysadminsLV.Asn1Parser;

namespace System.Security.Cryptography.X509Certificates {
    /// <summary>
    /// Represents a collection of <see cref="X509PolicyQualifier"/> objects.
    /// </summary>
    public class X509PolicyQualifierCollection : BasicCollection<X509PolicyQualifier> {
        /// <summary>
        /// Closes current collection state and makes it read-only. The collection cannot be modified further.
        /// </summary>
        public void Close() { IsReadOnly = true; }

        /// <summary>
        /// Encodes an array of <see cref="X509PolicyQualifier"/> to an ASN.1-encoded byte array.
        /// </summary>
        /// <returns>ASN.1-encoded byte array.</returns>
        public Byte[] Encode() {
            if (_list.Count == 0) { return null; }
            Int32 index = 1;
            List<Byte> rawData = new List<Byte>();
            foreach (X509PolicyQualifier qualifier in _list) {
                if (qualifier.Type == X509PolicyQualifierType.UserNotice) {
                    qualifier.NoticeNumber = index;
                    index++;
                }
                rawData.AddRange(qualifier.Encode());
            }
            return Asn1Utils.Encode(rawData.ToArray(), 48);
        }
        /// <summary>
        /// Decodes ASN.1 encoded byte array to an array of <see cref="X509PolicyQualifier"/> objects.
        /// </summary>
        /// <param name="rawData">ASN.1-encoded byte array.</param>
        /// <exception cref="InvalidDataException">
        /// The data in the <strong>rawData</strong> parameter is not valid array of <see cref="X509PolicyQualifier"/> objects.
        /// </exception>
        public void Decode(Byte[] rawData) {
            _list.Clear();
            Asn1Reader asn = new Asn1Reader(rawData);
            if (asn.Tag != 48) { throw new InvalidDataException("The data is invalid."); }
            asn.MoveNext();
            do {
                _list.Add(new X509PolicyQualifier(asn.GetTagRawData()));
            } while (asn.MoveNextCurrentLevel());
        }
    }
}