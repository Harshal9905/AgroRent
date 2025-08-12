package com.majorproj.agrorent;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.scheduling.annotation.EnableScheduling;

@SpringBootApplication
@EnableScheduling
public class AgrorentApplication {

	public static void main(String[] args) {
		SpringApplication.run(AgrorentApplication.class, args);
	}

}
