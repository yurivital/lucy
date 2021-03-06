﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil
//     Les modifications apportées à ce fichier seront perdues si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Lucy.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// Exten types
	/// </summary>
	public static class PrimitiveTypeExtension
	{
        /// <summary>
        /// Build a representaiton of byte array in hexadecimal string
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
		public static string ToHexaString(this byte[] array)
		{
            StringBuilder hexaString = new StringBuilder();
            foreach (byte b in array)
            {
                hexaString.AppendFormat("{0:X2}", b);
            }
            return hexaString.ToString();
		}

	}
}

