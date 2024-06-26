using Inventory.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Inventory.Models
{
    public class TransactionMetadata
    {
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? TransactionTypeId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Product? Product { get; set; }

    public Product? Name { get; set; }

    public virtual TransactionType? TransactionType { get; set; }
    }

    [MetadataType(typeof(TransactionMetadata))]
    
    public partial class Transaction
    {
        public static Transaction Create(YourDbContextClassName db,Transaction transaction)
        {
            transaction.CreateDate = DateTime.Now;
            transaction.UpdateDate = DateTime.Now;
            transaction.IsDeleted = false;
            db.Transactions.Add(transaction);
            db.SaveChanges();

            return transaction;
        }

         public static List<Transaction> GetAll(YourDbContextClassName db)
    {
        List<Transaction> returnThis = db.Transactions.Where(q=> q.IsDeleted != true).Include(p => p.Product).Include(t => t.TransactionType).ToList();
        
        
            foreach(var transaction in returnThis)
            {
                // ตั้งค่า TransactionId ให้เป็น null
                transaction.Product.Transactions = null;
            }
        return returnThis;
    }

    public static Transaction Delete(YourDbContextClassName db, int id)
        {
            Transaction transaction= GetById(db,id);
            transaction.IsDeleted = true;
            // db.Employees.Remove(employee); เป็นวิธีการลบแบบให้หายไปเลย
            db.Entry(transaction).State = EntityState.Modified; // Soft Delete
            db.SaveChanges();

            return transaction;
        }

    public static Transaction GetById(YourDbContextClassName db,int id)
        {
            Transaction? returnThis = db.Transactions.Where(q => q.Id == id && q.IsDeleted != true).FirstOrDefault();
            return returnThis ?? new Transaction();
        }
    }

    
        
    
}