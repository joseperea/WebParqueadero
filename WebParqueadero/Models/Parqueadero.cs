using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebParqueadero.Models
{
    public class Parqueadero
    {
        [Key]
        public Guid Id_Parq { get; set; }


        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Por favor ingrese {0} del parqueadero")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Maximo {1} y Menor a {2} caracteres")]
        [JsonProperty(PropertyName = "NombreEmpresa_Parq")]
        public string NombreEmpresa_Parq { get; set; }

        [Display(Name = "Nit")]
        [Required(ErrorMessage = "Por favor ingrese {0} del parqueadero")]
        [StringLength(18, MinimumLength = 3, ErrorMessage = "Maximo {1} y Menor a {2} caracteres")]
        [JsonProperty(PropertyName = "NitEmpresa_Parq")]
        public string NitEmpresa_Parq { get; set; }

        [StringLength(30, MinimumLength = 6, ErrorMessage = "Maximo {1} y Menor a {2} caracteres")]
        [Required(ErrorMessage = "Ingrese {0} del restaurante")]
        [Display(Name = "Dirección")]
        [JsonProperty(PropertyName = "Direccion_Parq")]
        public string Direccion_Parq { get; set; }

        [StringLength(30, MinimumLength = 6, ErrorMessage = "Maximo {1} y Menor a {2} caracteres")]
        [Required(ErrorMessage = "Por favor ingrese {0} del parqueadero")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefono")]
        [JsonProperty(PropertyName = "Telefono_Parq")]
        public string Telefono_Parq { get; set; }

        [Display(Name = "Hora de apertura")]
        [Required(ErrorMessage = "Por favor ingrese {0} del parqueadero")]
        [DisplayFormat(DataFormatString = "{0:hh:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time, ErrorMessage = "Por favor ingrese una hora de apertura")]
        [JsonProperty(PropertyName = "HoraApertura_Parq")]
        public DateTime HoraApertura_Parq { get; set; }

        [Display(Name = "Hora de cierre")]
        [Required(ErrorMessage = "Por favor ingrese {0} del parqueadero")]
        [DisplayFormat(DataFormatString = "{0:hh:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time, ErrorMessage = "Por favor ingrese {0} del parqueadero")]
        [JsonProperty(PropertyName = "HoraCierre_Parq")]
        public DateTime HoraCierre_Parq { get; set; }

        [Display(Name = "Minutos")]
        [JsonProperty(PropertyName = "PagoMinutos_Parq")]
        public int PagoMinutos_Parq { get; set; }

    
        [Required(ErrorMessage = "Por favor ingrese {0} del parqueadero")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Valor")]
        public decimal Valor_Parq { set; get; }

        [StringLength(30, MinimumLength = 6, ErrorMessage = "Maximo {1} y Menor a {2} caracteres")]
        [Required(ErrorMessage = "Por favor ingrese {0} del parqueadero")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Por favor ingrese un correo valido")]
        [Display(Name = "Correo")]
        public string Correo_Parq { get; set; }

        [StringLength(30, MinimumLength = 6, ErrorMessage = "Maximo {1} y Menor a {2} caracteres")]
        [Required(ErrorMessage = "Por favor ingrese {0} del parqueadero")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña del correo")]
        public string CorreoContra_Parq { get; set; }

        [Display(Name = "Impresoras")]
        public string Impresora_Parq { get; set; }

        [Display(Name = "Imprimir Ingreso Vehiculo")]
        public bool ImprimirIngreso_Parq { get; set; }

        [Display(Name = "Imprimir Factura")]
        public bool ImprimirFactura_Parq { get; set; }

        [Display(Name = "Modificar Valor Total?")]
        public bool ModificarValor_Parq { get; set; }

        [Display(Name = "Casillero?")]
        public bool Casillero { get; set; }

        [Display(Name = "Lavar?")]
        public bool Lavar { get; set; }


        //Relaciones
        public virtual ICollection<Documento> Documento { get; set; }

        public virtual ICollection<ParqueaderoUsuarioDetalle> ParqueaderoUsuarioDetalle { get; set; }

    }
}