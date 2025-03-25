import { Route, Routes, Navigate } from 'react-router-dom';
import LoginPage from './features/auth/LoginPage';
import CompanyListPage from './features/companies/CompanyListPage';
import CompanyForm from './features/companies/CompanyForm';
import ProtectedRoute from './components/ProtectedRoute';

function App() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />

      <Route
        path="/companies"
        element={
          <ProtectedRoute>
            <CompanyListPage />
          </ProtectedRoute>
        }
      />
      <Route
        path="/companies/new"
        element={
          <ProtectedRoute>
            <CompanyForm />
          </ProtectedRoute>
        }
      />
      <Route
        path="/companies/:id/edit"
        element={
          <ProtectedRoute>
            <CompanyForm />
          </ProtectedRoute>
        }
      />

      <Route path="*" element={<Navigate to="/companies" />} />
    </Routes>
  );
}

export default App;
