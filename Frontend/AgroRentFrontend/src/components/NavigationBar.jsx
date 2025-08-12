import React, { useEffect, useState } from "react";
import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import Button from "react-bootstrap/Button";
import logo from "../assets/AgroRent.png";
import { Link, useNavigate } from "react-router-dom";
import { getToken, getUserRole, removeToken } from "../services/UserServices";

function Navigationbar() {
  const navigate = useNavigate();
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [role, setRole] = useState(null);

  useEffect(() => {
    const token = getToken();
    setIsLoggedIn(!!token);
    if (token) {
      const userRole = getUserRole();
      setRole(userRole);
    } else {
      setRole(null);
    }
  }, []);

  const handleAuthAction = () => {
    if (isLoggedIn) {
      removeToken();
      setIsLoggedIn(false);
      setRole(null);
      navigate("/signin");
    } else {
      navigate("/signin");
    }
  };

  return (
    <Navbar bg="light" expand="lg" className="shadow-sm py-3 fixed-top">
      <Container>
        <Navbar.Brand
          as={Link}
          to="/"
          className="d-flex align-items-center gap-2 fw-bold fs-4"
        >
          <img src={logo} height="40" alt="Logo" className="d-inline-block" />
          AgroRent
        </Navbar.Brand>

        <Navbar.Toggle aria-controls="agro-navbar" />
        <Navbar.Collapse id="agro-navbar">
          <Nav className="ms-auto align-items-lg-center">
            {/* Always visible links */}
            <Nav.Link as={Link} to="/" className="px-3">Home</Nav.Link>
            <Nav.Link as={Link} to="/about" className="px-3">About Us</Nav.Link>
            <Nav.Link as={Link} to="/contact" className="px-3">Contact Us</Nav.Link>

            {/* Role-based links */}
            {role === "ROLE_FARMER" && (
              <>
                <Nav.Link as={Link} to="/my-equipments" className="px-3">My Equipments</Nav.Link>
                <Nav.Link as={Link} to="/my-bookings" className="px-3">My Bookings</Nav.Link>
                <Nav.Link as={Link} to="/payments" className="px-3">Payments</Nav.Link>
              </>
            )}

            {role === "ROLE_ADMIN" && (
              <>
                <Nav.Link as={Link} to="/admin/users" className="px-3">Users</Nav.Link>
                <Nav.Link as={Link} to="/admin/equipments" className="px-3">Equipments</Nav.Link>
              </>
            )}

            {/* Sign In / Logout */}
            <Button variant="primary" className="ms-3 mt-2 mt-lg-0" onClick={handleAuthAction}>
              {isLoggedIn ? "Logout" : "Sign Up"}
            </Button>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}

export default Navigationbar