import { BrowserRouter, Routes, Route } from "react-router-dom";
import AppNavbar from "./components/NavigationBar";
import { SignIn } from "./components/SignIn";
import SignUp from "./components/SignUp";
import Home from "./components/Home";

function App() {

  return (
    <BrowserRouter>
      <AppNavbar/>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/signin" element={<SignIn/>}/>
        <Route path="/signup" element={<SignUp/>}/>
      </Routes>
    </BrowserRouter>
  )
}

export default App
