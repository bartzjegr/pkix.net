﻿using System;

namespace PKI.CertificateServices.DB {
	/// <summary>
	/// Represents Certification Authority database row. This object contains only 4 base properties:
	/// <see cref="RowId"/>, <see cref="RequestId"/>, <see cref="ConfigString"/> and <see cref="Table"/>.
	/// Other properties should be added by using external means (for example, by using Add-Member cmdlet in
	///	Windows PowerShell).
	/// </summary>
	public class RequestRow {
		/// <summary>
		/// Gets or sets RowId which corresponds to row number in CA database.
		/// </summary>
		public Int32 RowId { get; set; }
		/// <summary>
		/// Gets or sets RequestId which corresponds to request ID number in CA database. This property is set to zero
		/// for non-request tables.
		/// </summary>
		public Int32 RequestId { get; set; }
		/// <summary>
		/// Gets or sets the configuration string of the CA server to which this object is related.
		/// </summary>
		public String ConfigString { get; set; }
		/// <summary>
		/// Gets or sets database table name.
		/// </summary>
		public TableList Table { get; set; }
	}
}
