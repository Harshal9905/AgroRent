package com.majorproj.agrorent.entities;

import java.util.Collection;
import java.util.List;

import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;

import jakarta.persistence.CascadeType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotNull;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

@Entity
@NoArgsConstructor
@Getter
@Setter
@Table(name="farmers")
@ToString(callSuper = true,exclude = {"equipmentList","bookings"})
public class Farmer extends BaseEntity implements UserDetails{
	 @Id
	 @GeneratedValue(strategy = GenerationType.IDENTITY)
	 private Long id;

	 @Column(length = 20, name = "first_name") 
	 private String firstName;
	 @Column(length = 30, name = "last_name") 
	 private String lastName;
	 @Column(length = 30, unique = true)
	 @NotNull
	 @Email
	 private String email;
	 @Column(nullable = false)
	 private String password;
	 @Enumerated(EnumType.STRING)
	 private Role role; // "ADMIN" or "FARMER" 
	 private boolean active=true;
	 
	// Razorpay identifiers
	 @Column(name = "razorpay_contact_id")
	 private String razorpayContactId;

	 @Column(name = "razorpay_fund_account_id")
	 private String razorpayFundAccountId;


	 @OneToMany(mappedBy = "owner",cascade = CascadeType.ALL, orphanRemoval = true)
	 private List<Equipment> equipmentList;

	 @OneToMany(mappedBy = "farmer",cascade = CascadeType.ALL, orphanRemoval = true)
	 private List<Booking> bookings;

	@Override
	public Collection<? extends GrantedAuthority> getAuthorities() {
		
		return List.of(new SimpleGrantedAuthority(this.role.name()));
	}

	@Override
	public String getUsername() {
		return this.email;
	}
	
	@Override
	public boolean isEnabled() {
		return this.active;
	}
	
	public void addEquipment(Equipment e) {
		this.equipmentList.add(e);
		e.setOwner(this);
	}
	
	public void removeEquipment(Equipment e) {
		this.equipmentList.remove(e);
		e.setOwner(null);
	}
	
	public void addBooking(Booking b) {
		this.bookings.add(b);
		b.setFarmer(this);
	}
	
	public void removeBooking(Booking b) {
		this.bookings.remove(b);
		b.setFarmer(null);
	}
}
