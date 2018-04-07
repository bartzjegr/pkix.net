﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using PKI.Exceptions;
using SysadminsLV.Asn1Parser;
using SysadminsLV.Asn1Parser.Universal;

namespace PKI.Utils.CLRExtensions {
    static class X509ExtensionExtensions {
        public static Byte[] Encode(this X509Extension extension) {
            if (extension == null) {
                throw new ArgumentNullException(nameof(extension));
            }
            if (String.IsNullOrEmpty(extension.Oid.Value)) {
                throw new UninitializedObjectException();
            }
            List<Byte> rawData = new List<Byte>(Asn1Utils.EncodeObjectIdentifier(extension.Oid));
            if (extension.Critical) {
                rawData.AddRange(Asn1Utils.EncodeBoolean(true));
            }

            rawData.AddRange(Asn1Utils.Encode(extension.RawData, (Byte)Asn1Type.OCTET_STRING));
            return Asn1Utils.Encode(rawData.ToArray(), 48);
        }
        public static X509Extension Decode(Byte[] rawData) {
            if (rawData == null) { throw new ArgumentNullException(nameof(rawData)); }

            Asn1Reader asn = new Asn1Reader(rawData);
            if (asn.Tag != 48) { throw new Asn1InvalidTagException(asn.Offset); }

            asn.MoveNext();
            if (asn.Tag != (Byte)Asn1Type.OBJECT_IDENTIFIER) { throw new Asn1InvalidTagException(asn.Offset); }

            Oid oid = new Asn1ObjectIdentifier(asn).Value;
            Boolean critical = false;
            asn.MoveNext();
            if (asn.Tag == (Byte)Asn1Type.BOOLEAN) {
                critical = Asn1Utils.DecodeBoolean(asn.GetTagRawData());
                asn.MoveNext();
            }
            if (asn.Tag != (Byte)Asn1Type.OCTET_STRING) { throw new Asn1InvalidTagException(asn.Offset); }

            return CryptographyUtils.ConvertExtension(new X509Extension(oid, asn.GetPayload(), critical));
        }
        public static Byte[] ExportBinaryData(this X509Extension extension) {
            if (String.IsNullOrEmpty(extension.Oid.Value)) { return null; }
            List<Byte> rawData = new List<Byte>(Asn1Utils.EncodeObjectIdentifier(extension.Oid));
            if (extension.Critical) {
                rawData.AddRange(Asn1Utils.EncodeBoolean(true));
            }
            rawData.AddRange(Asn1Utils.Encode(extension.RawData, (Byte)Asn1Type.OCTET_STRING));
            return Asn1Utils.Encode(rawData.ToArray(), 48);
        }
        public static void Import(this X509Extension extension, Byte[] rawData) {
            //extension.RawData = 
        }
    }
}
