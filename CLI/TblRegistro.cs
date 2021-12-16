using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CLI
{
    public partial class TblArquivo
    {
        public int ArquivoId { get; set; }
        public int QuantidadeRegistros { get; set; }
        public int QuantidadeRegistrosImportados { get; set; }
        [Column("DTH_Import", TypeName = "datetime")]
        public DateTime DthImport { get; set; }
    }
}
