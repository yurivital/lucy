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
	/// Represent a frament of text
	/// </summary>
	public class DocumentChunk
	{
        /// <summary>
        /// Get or set the content of the chunk
        /// </summary>
		public virtual string Text
		{
			get;
			set;
		}

        /// <summary>
        /// Get or set the chunk  metadata
        /// </summary>
        public virtual string Metadata
        {
            get;
            set;
        }

	}
}

