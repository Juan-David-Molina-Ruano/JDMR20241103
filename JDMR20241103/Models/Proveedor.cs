using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace JDMR20241103.Models
{
    public class Proveedor
    {
        public Proveedor()
        {
            DetalleProveedores = new List<DetalleProveedor>();
        }


        [Key]
        public int Id { get; set; }

        public virtual IList<DetalleProveedor> DetalleProveedores { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Solo se permiten letras")]
        [StringLength(60, ErrorMessage = "El campo {0} debe tener una longitud máxima de {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Solo se permiten números")]
        [StringLength(10, ErrorMessage = "El campo {0} debe tener una longitud máxima de {1} caracteres")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Solo se permiten letras")]
        [StringLength(200, ErrorMessage = "El campo {0} debe tener una longitud máxima de {1} caracteres")]
        public string Descripcion { get; set; }

    }
}
