import React from "react";
import { Formik, Form as FormikForm, Field, ErrorMessage } from "formik";
import * as Yup from "yup";
import { Container, Form, Button, Card } from "react-bootstrap";
import { toast, ToastContainer } from "react-toastify";
import { loginUser, addToken } from "../services/UserServices";
import { useNavigate } from "react-router-dom";

export function SignIn() {
  const navigate = useNavigate();

  const initialValues = {
    email: "",
    password: "",
  };

  const validationSchema = Yup.object().shape({
    email: Yup.string().email("Invalid email").required("Email required"),
    password: Yup.string().required("Password required"),
  });

  const handleSubmit = async (values, { setSubmitting }) => {
    try {
      const res = await loginUser(values);
      if (res.data.jwtToken) {
        addToken(res.data.jwtToken);
        toast.success(res.data.message || "Login successful!");
        window.location.href = "/";
      }
    } catch (error) {
      toast.error(error.response?.data?.message || "Login failed!");
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
        backgroundColor: "#f8f9fa",
        marginTop: "90px"
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
          <h2 className="mb-4 text-center">Sign In</h2>
          <Formik
            initialValues={initialValues}
            validationSchema={validationSchema}
            onSubmit={handleSubmit}
          >
            {({ isSubmitting }) => (
              <FormikForm>
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
                  {isSubmitting ? "Signing In..." : "SignIn"}
                </Button>

                <div className="text-center mt-3">
                  <span>Don't have an account? </span>
                  <span
                    className="text-primary"
                    style={{ cursor: "pointer" }}
                    onClick={() => navigate("/signup")}
                  >
                    Create new account
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
