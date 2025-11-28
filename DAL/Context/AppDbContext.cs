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
        public DbSet<KnowledgeBase> KnowledgeBases { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------------------
            // Business relationships (1 : M or 1:1)
            // ---------------------------
            modelBuilder.Entity<User>()
                .HasOne(u => u.Business)
                .WithMany(b => b.Users)
                .HasForeignKey(u => u.BusinessId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Business)
                .WithMany(b => b.Customers)
                .HasForeignKey(c => c.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuItem>()
                .HasOne(m => m.Business)
                .WithMany(b => b.MenuItems)
                .HasForeignKey(m => m.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Business)
                .WithMany(b => b.Orders)
                .HasForeignKey(o => o.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Business)
                .WithMany(b => b.Tickets)
                .HasForeignKey(t => t.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Interaction>()
                .HasOne(i => i.Business)
                .WithMany(b => b.Interactions)
                .HasForeignKey(i => i.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Business)
                .WithMany(b => b.Notifications)
                .HasForeignKey(n => n.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Business)
                .WithMany(b => b.Reports)
                .HasForeignKey(r => r.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1:1 Business - Setting
            modelBuilder.Entity<Business>()
                .HasOne(b => b.Setting)
                .WithOne(s => s.Business)
                .HasForeignKey<Setting>(s => s.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            // Business -> Subscriptions (1 : M)
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Business)
                .WithMany(b => b.Subscriptions)
                .HasForeignKey(s => s.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            // Business -> Integrations (1 : M)
            modelBuilder.Entity<Integration>()
                .HasOne(i => i.Business)
                .WithMany(b => b.Integrations)
                .HasForeignKey(i => i.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            // Business -> AuditLogs
            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.Business)
                .WithMany(b => b.AuditLogs)
                .HasForeignKey(a => a.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------
            // User relationships
            // ---------------------------
            // Interaction handled by User (Agent) — optional
            modelBuilder.Entity<Interaction>()
                .HasOne(i => i.HandledByUser)
                .WithMany(u => u.InteractionsHandled)
                .HasForeignKey(i => i.HandledByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Ticket assigned to User (optional)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.AssignedToUser)
                .WithMany(u => u.TicketsAssigned)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // AuditLog -> User (optional, system logs may have null user)
            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Notifications to User (optional)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------
            // Customer relationships
            // ---------------------------
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Customer)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Interaction>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.Interactions)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------
            // Orders & Items
            // ---------------------------
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany(mi => mi.OrderItems)
                .HasForeignKey(oi => oi.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);
            // ---------------------------
            // Subscription Relationships
            // ---------------------------

            // Subscription -> PaymentTransactions
            // ---------------------------
            modelBuilder.Entity<PaymentTransaction>()
                .HasOne(pt => pt.Subscription)
                .WithMany(s => s.PaymentTransactions)
                .HasForeignKey(pt => pt.SubscriptionId)
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


            // Feedback -> Tikects 
             modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Ticket)
                .WithMany(t => t.Feedbacks)
                .HasForeignKey(f => f.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // Message -> Interaction 
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Interaction)
                .WithMany(i => i.Messages)
                .HasForeignKey(m => m.InteractionId)
                .OnDelete(DeleteBehavior.Cascade);


            // Message -> Sentiment 
            modelBuilder.Entity<Message>()
            .HasOne(m => m.Sentiment)
            .WithOne(s => s.Message)
            .HasForeignKey<Sentiment>(s => s.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

            // Business -> KnowledgrBases
            modelBuilder.Entity<Business>()
            .HasOne(b => b.KnowledgeBases)
            .WithOne(k => k.Business)
            .HasForeignKey<KnowledgeBase>(k => k.BusinessId)
            .OnDelete(DeleteBehavior.Cascade);

            // Bussiness -> Settings 
            modelBuilder.Entity<Business>()
                .HasOne(b => b.Setting)
                .WithOne(s => s.Business)
                .HasForeignKey<Setting>(o => o.SettingId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
