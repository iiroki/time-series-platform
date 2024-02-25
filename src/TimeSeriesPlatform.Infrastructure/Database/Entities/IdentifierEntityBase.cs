using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Iiroki.TimeSeriesPlatform.Infrastructure.Database.Entities;

[PrimaryKey(nameof(Id))]
internal class IdentifierEntityBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
}
