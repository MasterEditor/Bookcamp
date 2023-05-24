import React from "react";
import { Route, Routes } from "react-router-dom";
import ProtectedRout from "./components/ProtectedRout";
import { ADMIN, UNKNOWN, USER } from "./constants/roles";
import AddBook from "./pages/Admin/AddBook";
import AdminBooks from "./pages/Admin/AdminBooks";
import AdminUsers from "./pages/Admin/AdminUsers";
import Books from "./pages/Books";
import Forbidden from "./pages/Forbidden";
import Login from "./pages/Login";
import MainPage from "./pages/MainPage";
import NotFound from "./pages/NotFound";
import OneBookNew from "./pages/OneBookNew";
import Profile from "./pages/Profile";
import SignUp from "./pages/SignUp";

function App() {
  return (
    <Routes>
      <Route path="/" element={<MainPage />} />
      <Route path="/login" element={<Login />} />
      <Route path="/signup" element={<SignUp />} />
      <Route path="/books" element={<Books />} />

      <Route element={<ProtectedRout allowedRoles={[USER]} />}>
        <Route path="/profile" element={<Profile />} />
      </Route>

      <Route element={<ProtectedRout allowedRoles={[USER, ADMIN, UNKNOWN]} />}>
        <Route path="/book/:id" element={<OneBookNew />} />
      </Route>

      <Route element={<ProtectedRout allowedRoles={[ADMIN]} />}>
        <Route path="/admin-books" element={<AdminBooks />} />
      </Route>
      <Route element={<ProtectedRout allowedRoles={[ADMIN]} />}>
        <Route path="/users" element={<AdminUsers />} />
      </Route>
      <Route element={<ProtectedRout allowedRoles={[ADMIN]} />}>
        <Route path="/add-book" element={<AddBook />} />
      </Route>

      <Route path="/unauthorized" element={<Forbidden />} />
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
}

export default App;
