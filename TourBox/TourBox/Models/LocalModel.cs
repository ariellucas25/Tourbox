using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace TourBox.Models
{
    [DataTable("locais")]
    public class LocalModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("idCidadeFk")]
        public string IdCidadeFk { get; set; }

        [JsonProperty("urlFoto")]
        public string PhotoName { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("endereco")]
        public string Endereco { get; set; }

        [JsonProperty("visibilidade")]
        public bool Visibilidade { get; set; }

        [JsonProperty("autor")]
        public string autor { get; set; }

        [Version]
        public string Version { get; set; }

    }
}