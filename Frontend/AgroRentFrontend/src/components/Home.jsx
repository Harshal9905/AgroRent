import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getAllEquipment, getSingleEquipment } from '../services/EquipmentService';
import './Home.css';
import { getToken } from "../services/UserServices"
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';

const Home = () => {
    const [equipment, setEquipment] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [selectedEquipment, setSelectedEquipment] = useState(null);
    const [showModal, setShowModal] = useState(false);
    const [modalLoading, setModalLoading] = useState(false);
    const [bookingData, setBookingData] = useState({
        startDate: '',
        endDate: ''
    });
    const navigate = useNavigate();

    useEffect(() => {
        const token = getToken();
        if (!token) {
            navigate('/signin');
        } else {
            fetchEquipment();
        }
    }, [navigate]);

    const fetchEquipment = async () => {
        try {
            setLoading(true);
            const response = await getAllEquipment();
            setEquipment(response.data);
            setError(null);
        } catch (err) {
            console.error('Error fetching equipment:', err);
            setError('Failed to load equipment. Please try again later.');
        } finally {
            setLoading(false);
        }
    };

    const handleRentClick = async (equipmentId) => {
        try {
            setModalLoading(true);
            const response = await getSingleEquipment(equipmentId);
            setSelectedEquipment(response.data);
            setShowModal(true);
        } catch (err) {
            console.error('Error fetching equipment details:', err);
            alert('Failed to load equipment details. Please try again.');
        } finally {
            setModalLoading(false);
        }
    };

    const handleCloseModal = () => {
        setShowModal(false);
        setSelectedEquipment(null);
        setBookingData({ startDate: '', endDate: '' });
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setBookingData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleBookEquipment = () => {
        if (!bookingData.startDate || !bookingData.endDate) {
            alert('Please select both start and end dates.');
            return;
        }

        const startDate = new Date(bookingData.startDate);
        const endDate = new Date(bookingData.endDate);
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        if (startDate < today) {
            alert('Start date cannot be in the past.');
            return;
        }

        if (endDate <= startDate) {
            alert('End date must be after start date.');
            return;
        }

        // Calculate total days and price
        const daysDiff = Math.ceil((endDate - startDate) / (1000 * 60 * 60 * 24));
        const totalPrice = daysDiff * selectedEquipment.rentalPrice;

        // Here you would typically make an API call to book the equipment
        console.log('Booking equipment:', {
            equipmentId: selectedEquipment.id,
            equipmentName: selectedEquipment.name,
            startDate: bookingData.startDate,
            endDate: bookingData.endDate,
            totalDays: daysDiff,
            totalPrice: totalPrice
        });

        alert(`Booking successful! Total price: ₹${totalPrice} for ${daysDiff} days.`);
        handleCloseModal();
    };

    if (loading) {
        return (
            <div className="loading-container">
                <div className="loading-spinner"></div>
                <p>Loading equipment...</p>
            </div>
        );
    }

    if (error) {
        return (
            <div className="error-container">
                <p className="error-message">{error}</p>
                <button onClick={fetchEquipment} className="retry-button">
                    Try Again
                </button>
            </div>
        );
    }

    return (
        <div className="home-container mt-5">
            <div className="home-header">
                <h1>Available Equipment</h1>
                <p>Browse and rent agricultural equipment for your farming needs</p>
            </div>
            
            <div className="equipment-grid">
                {equipment.map((item) => (
                    <div key={item.id} className="equipment-card">
                        <div className="card-image-container">
                            <img 
                                src={item.imageUrl} 
                                alt={item.name}
                                className="equipment-image"
                                onError={(e) => {
                                    e.target.src = 'https://via.placeholder.com/300x200?text=Equipment+Image';
                                }}
                            />
                            <div className={`availability-badge ${item.available ? 'available' : 'unavailable'}`}>
                                {item.available ? 'Available' : 'Unavailable'}
                            </div>
                        </div>
                        
                        <div className="card-content">
                            <h3 className="equipment-name">{item.name}</h3>
                            <p className="equipment-description">{item.description}</p>
                            
                            <div className="card-footer">
                                <div className="price-container">
                                    <span className="price-label">Rental Price:</span>
                                    <span className="price">₹{item.rentalPrice}/day</span>
                                </div>
                                
                                <button 
                                    className={`rent-button ${!item.available ? 'disabled' : ''}`}
                                    disabled={!item.available}
                                    onClick={() => handleRentClick(item.id)}
                                >
                                    {item.available ? 'Rent Now' : 'Not Available'}
                                </button>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
            
            {equipment.length === 0 && !loading && (
                <div className="no-equipment">
                    <p>No equipment available at the moment.</p>
                </div>
            )}

            {/* Equipment Details Modal */}
            <Modal show={showModal} onHide={handleCloseModal} size="lg" centered>
                <Modal.Header closeButton>
                    <Modal.Title>Equipment Details</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {modalLoading ? (
                        <div className="text-center">
                            <div className="loading-spinner"></div>
                            <p>Loading equipment details...</p>
                        </div>
                    ) : selectedEquipment ? (
                        <div className="equipment-details">
                            <div className="row">
                                <div className="col-md-6">
                                    <img 
                                        src={selectedEquipment.imageUrl} 
                                        alt={selectedEquipment.name}
                                        className="img-fluid rounded"
                                        style={{ maxHeight: '300px', width: '100%', objectFit: 'cover' }}
                                        onError={(e) => {
                                            e.target.src = 'https://via.placeholder.com/400x300?text=Equipment+Image';
                                        }}
                                    />
                                </div>
                                <div className="col-md-6">
                                    <h4>{selectedEquipment.name}</h4>
                                    <p className="text-muted">{selectedEquipment.description}</p>
                                    <div className="mb-3">
                                        <strong>Rental Price:</strong> ₹{selectedEquipment.rentalPrice}/day
                                    </div>
                                    <div className="mb-3">
                                        <strong>Status:</strong> 
                                        <span className={`badge ${selectedEquipment.available ? 'bg-success' : 'bg-danger'} ms-2`}>
                                            {selectedEquipment.available ? 'Available' : 'Unavailable'}
                                        </span>
                                    </div>
                                    
                                    {selectedEquipment.available && (
                                        <div className="booking-form">
                                            <h5 className="mb-3">Book This Equipment</h5>
                                            <Form>
                                                <Form.Group className="mb-3">
                                                    <Form.Label>Start Date</Form.Label>
                                                    <Form.Control
                                                        type="date"
                                                        name="startDate"
                                                        value={bookingData.startDate}
                                                        onChange={handleInputChange}
                                                        min={new Date().toISOString().split('T')[0]}
                                                    />
                                                </Form.Group>
                                                <Form.Group className="mb-3">
                                                    <Form.Label>End Date</Form.Label>
                                                    <Form.Control
                                                        type="date"
                                                        name="endDate"
                                                        value={bookingData.endDate}
                                                        onChange={handleInputChange}
                                                        min={bookingData.startDate || new Date().toISOString().split('T')[0]}
                                                    />
                                                </Form.Group>
                                            </Form>
                                        </div>
                                    )}
                                </div>
                            </div>
                        </div>
                    ) : null}
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleCloseModal}>
                        Close
                    </Button>
                    {selectedEquipment && selectedEquipment.available && (
                        <Button variant="primary" onClick={handleBookEquipment}>
                            Book Equipment
                        </Button>
                    )}
                </Modal.Footer>
            </Modal>
        </div>
    );
};

export default Home;
