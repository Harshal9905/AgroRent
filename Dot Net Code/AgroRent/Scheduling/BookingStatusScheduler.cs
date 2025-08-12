using AgroRent.Repositories;
using AgroRent.Models;

namespace AgroRent.Scheduling
{
    public class BookingStatusScheduler : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BookingStatusScheduler> _logger;
        private readonly TimeSpan _period = TimeSpan.FromMinutes(30); // Check every 30 minutes

        public BookingStatusScheduler(IServiceProvider serviceProvider, ILogger<BookingStatusScheduler> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UpdateBookingStatusesAsync();
                    await Task.Delay(_period, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating booking statuses");
                    await Task.Delay(_period, stoppingToken);
                }
            }
        }

        private async Task UpdateBookingStatusesAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingRepository>();

            var now = DateTime.Now;

            // Update completed bookings
            var pendingBookings = await bookingRepository.GetByStatusAsync(BookingStatus.ACCEPTED);
            foreach (var booking in pendingBookings)
            {
                if (booking.EndDate.Date <= now.Date)
                {
                    booking.Status = BookingStatus.COMPLETED;
                    booking.UpdatedOn = now;
                    await bookingRepository.UpdateAsync(booking);
                    _logger.LogInformation($"Updated booking {booking.Id} to COMPLETED");
                }
            }

            // Update expired pending bookings
            var expiredPendingBookings = await bookingRepository.GetByStatusAsync(BookingStatus.PENDING);
            foreach (var booking in expiredPendingBookings)
            {
                if (booking.StartDate.Date < now.Date)
                {
                    booking.Status = BookingStatus.CANCELLED;
                    booking.UpdatedOn = now;
                    await bookingRepository.UpdateAsync(booking);
                    _logger.LogInformation($"Updated expired booking {booking.Id} to CANCELLED");
                }
            }
        }
    }
}
