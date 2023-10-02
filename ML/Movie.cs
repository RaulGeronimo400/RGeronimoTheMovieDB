using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Movie
    {
        public bool Adult { get; set; }
        public string Portada { get; set; }
        public List<int> Generos { get; set; }
        public int IdPelicula { get; set; }
        public string IdiomaOriginal { get; set; }
        public string TituloOriginal { get; set; }
        public string Sinopsis { get; set; }
        public double Popularidad { get; set; }
        public string DireccionPortada { get; set; }
        public string FechaEstreno { get; set; }
        public string Titulo { get; set; }
        public bool Video { get; set; }
        public int Puntuacion { get; set; }
        public int TotalVotos { get; set; }
        public List<object> Peliculas { get; set; }
    }
}
