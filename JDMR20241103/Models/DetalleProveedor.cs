using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace JDMR20241103.Models
{
    public class DetalleProveedor
    {
        [Key]
        public int Id { get; set; }

        public int ProveedorId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Solo se permiten letras")]
        [StringLength(100)]
        public string Calle { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Solo se permiten letras")]
        [StringLength(100)]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Solo se permiten letras")]
        [StringLength(100)]
        public string Estado { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Solo se permiten letras")]
        [StringLength(100)]
        public string Pais { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Solo se permiten letras")]
        [StringLength(100)]
        public string CodigoPostal { get; set; } 

        public virtual Proveedor Proveedor { get; set; } = null!;
    }
}
