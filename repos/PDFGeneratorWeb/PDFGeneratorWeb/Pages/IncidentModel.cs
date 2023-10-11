using System.ComponentModel.DataAnnotations;

namespace PDFGeneratorWeb.Pages
{
    public class IncidentModel
    {
        [Key]
        public int incident_id { get; set; }
        public string incident_desc { get; set; }
        public int ordre {  get; set; }
        public string etat { get; set; }

    }
}
