package com.majorproj.agrorent.scheduling;

import java.util.List;

import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;
import org.springframework.transaction.annotation.Transactional;

import com.majorproj.agrorent.dao.BookingDao;
import com.majorproj.agrorent.entities.Booking;
import com.majorproj.agrorent.entities.BookingStatus;

import lombok.AllArgsConstructor;
import lombok.extern.slf4j.Slf4j;

@Component
@AllArgsConstructor
@Slf4j
public class BookingStatusScheduler {
	private final BookingDao bookingDao;
	
	
	@Scheduled(cron = "0 0 1 * * *")
	@Transactional
	public void completeBookings() {
		List<Booking> bookings=bookingDao.findBookingsToComplete();
		for(Booking b: bookings) {
			b.setStatus(BookingStatus.COMPLETED);
		}
		
		bookingDao.saveAll(bookings);
		log.info("Updated "+bookings.size()+" bookings status to COMPLETED");
	}
}
