package com.majorproj.agrorent.dto;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
public class EquipmentRespDto {
	private Long id;
    private String name;
    private String description;
    private String imageUrl;
    private double rentalPrice;
    private boolean available;
}
