package com.majorproj.agrorent.entities;
import java.util.List;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

@Entity
@Getter
@Setter
@NoArgsConstructor
@Table(name = "equipments")
@ToString(callSuper = true,exclude = {"bookings"})
public class Equipment extends BaseEntity{
	@Id @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
	@Column(length = 30, name = "equipment_name")
    private String name;
    private String description;
    private String imageUrl;
    @Column(nullable = false)
    private double rentalPrice;
    private boolean available=true;
//    private String type;
    private String cloudinaryPublicId;

    @ManyToOne
    private Farmer owner;

    @OneToMany(mappedBy = "equipment")
    private List<Booking> bookings;
    
    public void addBooking(Booking b) {
		this.bookings.add(b);
		b.setEquipment(this);
	}
	
	public void removeBooking(Booking b) {
		this.bookings.remove(b);
		b.setEquipment(null);
	}
}
