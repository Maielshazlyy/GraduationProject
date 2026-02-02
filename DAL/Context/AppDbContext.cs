using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
  public  class AppDbContext: IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public DbSet<Business> Businesses { get; set; }
       // public DbSet<User> Users { get; set; }
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
        public DbSet<Integration> Integrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------------------
            // Primary Keys Configuration
            // ---------------------------
            modelBuilder.Entity<Business>().HasKey(b => b.Id);
            modelBuilder.Entity<Customer>().HasKey(c => c.CustomerId);
            modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
            modelBuilder.Entity<OrderItem>().HasKey(oi => oi.OrderItemId);
            modelBuilder.Entity<Ticket>().HasKey(t => t.Id);
            modelBuilder.Entity<Interaction>().HasKey(i => i.InteractionId);
            modelBuilder.Entity<Message>().HasKey(m => m.MessageId);
            modelBuilder.Entity<Notification>().HasKey(n => n.NotificationId);
            modelBuilder.Entity<Report>().HasKey(r => r.Id);
            modelBuilder.Entity<Feedback>().HasKey(f => f.FeedbackId);
            modelBuilder.Entity<MenuItem>().HasKey(m => m.MenuItemId);
            modelBuilder.Entity<Setting>().HasKey(s => s.SettingId);
            modelBuilder.Entity<Subscription>().HasKey(s => s.Id);
            modelBuilder.Entity<PaymentTransaction>().HasKey(pt => pt.Id);
            modelBuilder.Entity<Integration>().HasKey(i => i.Id);
            modelBuilder.Entity<AuditLog>().HasKey(a => a.AuditLogId);
            modelBuilder.Entity<KnowledgeBase>().HasKey(k => k.KnowledgeBaseId);
            modelBuilder.Entity<Sentiment>().HasKey(s => s.SentimentId);

            // ---------------------------
            // Business relations (1:M / 1:1)
            // ---------------------------
            modelBuilder.Entity<User>()
                .HasOne(u => u.Business)
                .WithMany(b => b.Users)
                .HasForeignKey(u => u.BusinessId)
                .IsRequired(false) // جعل BusinessId اختياري
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

            modelBuilder.Entity<KnowledgeBase>()
                .HasOne(k => k.Business)
                .WithMany(b => b.KnowledgeBases)
                .HasForeignKey(k => k.BusinessId)
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
            // User relations (agent)
            // ---------------------------
            modelBuilder.Entity<Interaction>()
                .HasOne(i => i.HandledByUser)
                .WithMany(u => u.InteractionsHandled)
                .HasForeignKey(i => i.HandledByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.AssignedToUser)
                .WithMany(u => u.TicketsAssigned)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // ---------------------------
            // Customer relations
            // ---------------------------
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Customer)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Interaction>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.Interactions)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Customer)
                .WithMany(c => c.Feedbacks)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------
            // Order / OrderItem relations
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
            // Ticket / Feedback
            // ---------------------------
            modelBuilder.Entity<Feedback>()
           .HasOne(f => f.Ticket)
          .WithMany(t => t.Feedbacks)
          .HasForeignKey(f => f.TicketId)
          .OnDelete(DeleteBehavior.Restrict); // تعديل هنا بدلاً من Cascade

            // ---------------------------
            // Message / Interaction
            // ---------------------------
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Interaction)
                .WithMany(i => i.Messages)
                .HasForeignKey(m => m.InteractionId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------
            // Message <-> Sentiment (1:1)
            // ---------------------------
            modelBuilder.Entity<Sentiment>()
                .HasOne(s => s.Message)
                .WithOne(m => m.Sentiment)
                .HasForeignKey<Sentiment>(s => s.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------
            // PaymentTransaction -> Subscription
            // ---------------------------
            modelBuilder.Entity<PaymentTransaction>()
                .HasOne(pt => pt.Subscription)
                .WithMany(s => s.PaymentTransactions)
                .HasForeignKey(pt => pt.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            var decimalProps = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?));

            foreach (var property in decimalProps)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }
        }
    }
}
