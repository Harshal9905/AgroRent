import React from "react";
import { Formik, Form as FormikForm, Field, ErrorMessage } from "formik";
import * as Yup from "yup";
import { Container, Row, Col, Form, Button, Card } from "react-bootstrap";
import { toast, ToastContainer } from "react-toastify";
import { registerUser } from "../services/UserServices";
import { useNavigate } from "react-router-dom";

function SignUp() {
  const navigate = useNavigate();

  const initialValues = {
    firstName: "",
    lastName: "",
    email: "",
    password: "",
  };

  const validationSchema = Yup.object().shape({
    firstName: Yup.string().required("Firstname required"),
    lastName: Yup.string().required("Lastname required"),
    email: Yup.string().email("Invalid email").required("Email required"),
    password: Yup.string()
      .matches(
        /((?=.*\d)(?=.*[a-z])(?=.*[#@$*]).{5,20})/,
        "Invalid password format!!!!"
      )
      .required("Password required"),
  });

  const handleSubmit = async (values, { setSubmitting, resetForm }) => {
    try {
      const res = await registerUser(values);
      toast.success(res.data.message || "Registration successful!");
      resetForm();
      navigate("/signin");
    } catch (error) {
      toast.error(error.response?.data?.message || "Registration failed!");
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Container
      fluid
      className="d-flex justify-content-center align-items-center"
      style={{
        minHeight: "100vh",
        paddingTop: "100px",
        backgroundColor: "#f8f9fa",
      }}
    >
      <Card
        style={{
          width: "100%",
          maxWidth: "500px",
          padding: "3rem",
          background: "rgba(255, 255, 255, 0.85)",
          border: "0.1rem solid black",
          borderRadius: "2rem",
          boxShadow: "0 10px 40px rgba(0, 0, 0, 0.3)",
          backdropFilter: "blur(10px)",
        }}
        className="shadow-sm border-0"
      >
        <Card.Body>
          <h2 className="mb-4 text-center">Create Account</h2>
          <Formik
            initialValues={initialValues}
            validationSchema={validationSchema}
            onSubmit={handleSubmit}
          >
            {({ isSubmitting }) => (
              <FormikForm>
                <Row>
                  <Col>
                    <Form.Group className="mb-3">
                      <Form.Label>First Name</Form.Label>
                      <Field name="firstName" className="form-control" />
                      <ErrorMessage
                        name="firstName"
                        component="div"
                        className="text-danger small"
                      />
                    </Form.Group>
                  </Col>
                  <Col>
                    <Form.Group className="mb-3">
                      <Form.Label>Last Name</Form.Label>
                      <Field name="lastName" className="form-control" />
                      <ErrorMessage
                        name="lastName"
                        component="div"
                        className="text-danger small"
                      />
                    </Form.Group>
                  </Col>
                </Row>

                <Form.Group className="mb-3">
                  <Form.Label>Email</Form.Label>
                  <Field name="email" type="email" className="form-control" />
                  <ErrorMessage
                    name="email"
                    component="div"
                    className="text-danger small"
                  />
                </Form.Group>

                <Form.Group className="mb-4">
                  <Form.Label>Password</Form.Label>
                  <Field
                    name="password"
                    type="password"
                    className="form-control"
                  />
                  <ErrorMessage
                    name="password"
                    component="div"
                    className="text-danger small"
                  />
                </Form.Group>

                <Button
                  variant="primary"
                  type="submit"
                  className="w-100"
                  disabled={isSubmitting}
                >
                  {isSubmitting ? "Registering..." : "Sign Up"}
                </Button>
                <div className="text-center mt-3">
                  <span>Already have an account? </span>
                  <span
                    className="text-primary"
                    style={{ cursor: "pointer" }}
                    onClick={() => navigate("/signin")}
                  >
                    SignIn
                  </span>
                </div>
              </FormikForm>
            )}
          </Formik>
        </Card.Body>
      </Card>
      <ToastContainer position="top-center" />
    </Container>
  );
}

export default SignUp;
