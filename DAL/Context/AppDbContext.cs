using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
  public  class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Interaction> Interactions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Sentiment> Sentiments { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Business → Users (1:M)
            // ------------------------
            modelBuilder.Entity<User>()
                .HasOne(u => u.Business)
                .WithMany(b => b.Users)
                .HasForeignKey(u => u.BusinessIdFk)
                .OnDelete(DeleteBehavior.Cascade);


            // ------------------------
            // Business → Customers (1:M)
            // ------------------------
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Business)
                .WithMany(b => b.Customers)
                .HasForeignKey(c => c.BusinessIdFk)
                .OnDelete(DeleteBehavior.Cascade);


            // ------------------------
            // Business → MenuItems (1:M)
            // ------------------------
            modelBuilder.Entity<MenuItem>()
                .HasOne(m => m.Business)
                .WithMany(b => b.MenuItems)
                .HasForeignKey(m => m.BusinessIdFk)
                .OnDelete(DeleteBehavior.Cascade);


            // ------------------------
            // Business → Orders (1:M)
            // ------------------------
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Business)
                .WithMany(b => b.Orders)
                .HasForeignKey(o => o.BusinessIdFk)
                .OnDelete(DeleteBehavior.Cascade);


            // ------------------------
            // Business → Tickets (1:M)
            // ------------------------
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Business)
                .WithMany(b => b.Tickets)
                .HasForeignKey(t => t.BusinessIdFk)
                .OnDelete(DeleteBehavior.Cascade);


            // ------------------------
            // Business → Interactions (1:M)
            // ------------------------
            modelBuilder.Entity<Interaction>()
                .HasOne(i => i.Business)
                .WithMany(b => b.Interactions)
                .HasForeignKey(i => i.BusinessIdFk)
                .OnDelete(DeleteBehavior.Cascade);


            // ------------------------
            // Business → Notifications (1:M)
            // ------------------------
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Business)
                .WithMany(b => b.Notifications)
                .HasForeignKey(n => n.BusinessIdFk)
                .OnDelete(DeleteBehavior.Cascade);


            // ------------------------
            // Business → Reports (1:M)
            // ------------------------
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Business)
                .WithMany(b => b.Reports)
                .HasForeignKey(r => r.BusinessIdFk)
                .OnDelete(DeleteBehavior.Cascade);


            // ------------------------
            // Business → Settings (1:1)
            // ------------------------
            modelBuilder.Entity<Business>()
                .HasOne(b => b.Settings)
                .WithOne(s => s.Business)
                .HasForeignKey<Setting>(s => s.BusinessIdFk)
                .OnDelete(DeleteBehavior.Cascade);


            // ------------------------
            // Business → Subscription (1:1)
            // ------------------------
            modelBuilder.Entity<Business>()
                .HasOne(b => b.Subscriptions)
                .WithOne(s => s.Business)
                .HasForeignKey<Subscription>(s => s.BusinessIdFk)
                .OnDelete(DeleteBehavior.Cascade);

            // USER RELATIONSHIPS
            // ==============================

            // 1) User → Business  (Many-to-One)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Business)
                .WithMany(b => b.Users)
                .HasForeignKey(u => u.BusinessIdFk)
                .OnDelete(DeleteBehavior.Restrict);
            // 2 ) User ↔ InteractionsHandled (One-to-Many)
            // -----------------------------
            modelBuilder.Entity<Interaction>()
                .HasOne(i => i.HandledByUser)          
                .WithMany(u => u.InteractionsHandled)
                .HasForeignKey(i => i.HandledByUserId)
                .OnDelete(DeleteBehavior.NoAction);
            // 3 ) User ↔ TicketsAssigned (One-to-Many)
            // -----------------------------
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.AssignedToUser)          
                .WithMany(u => u.TicketsAssigned)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.NoAction);
            //4)User ↔ AuditLogs (One-to-Many)
            // -----------------------------
            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.UserIdFk)
                .OnDelete(DeleteBehavior.Cascade);

            // 5) // User ↔ Notifications (One-to-Many)
            // -----------------------------
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserIdFk)
                .OnDelete(DeleteBehavior.Cascade);



            //Order → OrderItem (One-to-Many)
            modelBuilder.Entity<Order>()
                 .HasMany(o => o.OrderItems)
                 .WithOne(oi => oi.Order)
                 .HasForeignKey(oi => oi.OrderId);

            //OrderItem → MenuItem (Many-to-One)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany(mi => mi.OrderItems)
                .HasForeignKey(oi => oi.MenuItemId);

            //Subscription → PaymentTransaction (One-to-Many)
            modelBuilder.Entity<Subscription>()
              .HasMany(s => s.PaymentTransactions)
              .WithOne(p => p.Subscription)
              .HasForeignKey(p => p.SubscriptionId);

            //Customer → Order (One-to-Many)
            modelBuilder.Entity<Customer>()
              .HasMany(c => c.Orders)
              .WithOne(o => o.Customer)
              .HasForeignKey(o => o.CustomerId);

            // Business → Integrations
            modelBuilder.Entity<Business>()
                .HasMany(b => b.Integrations)
                .WithOne(i => i.Business)
                .HasForeignKey(i => i.BusinessId);

            // Business → AuditLogs
            modelBuilder.Entity<Business>()
                .HasMany(b => b.AuditLogs)
                .WithOne(a => a.Business)
                .HasForeignKey(a => a.BusinessId);

        }
    }
}
