package com.majorproj.agrorent.services;

import java.util.List;
import java.util.UUID;

import org.modelmapper.ModelMapper;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.majorproj.agrorent.customeExceeption.ApiException;
import com.majorproj.agrorent.dao.EquipmentDao;
import com.majorproj.agrorent.dao.FarmerDao;
import com.majorproj.agrorent.dto.ApiResponse;
import com.majorproj.agrorent.dto.EquipmentDto;
import com.majorproj.agrorent.dto.EquipmentRespDto;
import com.majorproj.agrorent.dto.EquipmentUpdateDto;
import com.majorproj.agrorent.entities.Equipment;
import com.majorproj.agrorent.entities.Farmer;

import jakarta.validation.Valid;
import lombok.AllArgsConstructor;

@Service
@Transactional
@AllArgsConstructor
public class EquipmentServiceImpl implements EquipmentService {
	private final EquipmentDao equipmentDao;
	private final FarmerDao farmerDao;
	private final ModelMapper mapper;
	private final ImageService imageService;
	
	
	@Override
	public ApiResponse addEquipment(EquipmentDto dto, String email) {
		Farmer farmer= farmerDao.findByEmail(email).orElseThrow(()->new ApiException("Invalid farmer"));
		
		Equipment equipment=mapper.map(dto, Equipment.class);
		equipment.setOwner(farmer);
		farmer.addEquipment(equipment);
		
		String fileName=UUID.randomUUID().toString();
		String imageUrl=imageService.uploadImage(dto.getImage(), fileName);
		
		equipment.setImageUrl(imageUrl);
		equipment.setCloudinaryPublicId(fileName);
		
		if(equipmentDao.save(equipment)==null) {
			throw new ApiException("Unable to add new Equipment");
		}
		
		return new ApiResponse("Equipment added successfully");
	}


	@Override
	public List<EquipmentRespDto> getAllEquipments(String email) {
		List<Equipment> equipments=equipmentDao.findAllExceptOwnerEmail(email);
		return equipments.stream()
				.map(equipment->mapper.map(equipment,EquipmentRespDto.class ))
				.toList();
	}


	@Override
	public ApiResponse updateEquipment(Long equipmentId, @Valid EquipmentUpdateDto dto) {
		
		Equipment equipment=equipmentDao.findById(equipmentId).orElseThrow(()->new ApiException("Invalid equipment Id"));
		
		equipment.setName(dto.getName());
		equipment.setDescription(dto.getDescription());
		equipment.setRentalPrice(dto.getRentalPrice());
		
		if(dto.getImage()!=null) {
			imageService.deleteImage(equipment.getCloudinaryPublicId());
			String fileName=UUID.randomUUID().toString();
			String imageUrl=imageService.uploadImage(dto.getImage(), fileName);
			equipment.setImageUrl(imageUrl);
			equipment.setCloudinaryPublicId(fileName);
		}
		return new ApiResponse("Equipment Data updated!!");
	}


	@Override
	public ApiResponse deleteEquipment(Long equipmentId) {
		
		Equipment equipment=equipmentDao.findById(equipmentId).orElseThrow(()->new ApiException("Invalid equipment Id"));
		equipment.getOwner().removeEquipment(equipment);
		imageService.deleteImage(equipment.getCloudinaryPublicId()); 
		equipmentDao.delete(equipment);
		
		return new ApiResponse("Equipmet deleted successfully");
	}


	@Override
	public EquipmentRespDto getEquipmentById(Long equipmentId) {
		Equipment equipment=equipmentDao.findById(equipmentId).orElseThrow(()->new ApiException("Invalid Equipment Id"));
		return mapper.map(equipment, EquipmentRespDto.class);
	}


	@Override
	public List<EquipmentRespDto> getUsersEquipment(String email) {
		
		return equipmentDao.findOwnerEquipments(email);
	}


	@Override
	public ApiResponse deleteEquipment(String email) {
		Equipment equipment=equipmentDao.findByOwnerEmail(email).orElseThrow(()->new ApiException("Invalid user"));
		equipment.getOwner().removeEquipment(equipment);
		imageService.deleteImage(equipment.getCloudinaryPublicId()); 
		equipmentDao.delete(equipment);
		
		return new ApiResponse("Equipmet deleted successfully");
	}
	
}
