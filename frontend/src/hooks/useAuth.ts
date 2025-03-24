import { useEffect, useState } from 'react';

export function useAuth() {
  const [token, setTokenState] = useState<string | null>(localStorage.getItem('token'));

  useEffect(() => {
    const handler = () => setTokenState(localStorage.getItem('token'));
    window.addEventListener('storage', handler);
    return () => window.removeEventListener('storage', handler);
  }, []);

  const setToken = (value: string) => {
    localStorage.setItem('token', value);
    setTokenState(value); // trigger re-render
  };

  const clearToken = () => {
    localStorage.removeItem('token');
    setTokenState(null); // trigger re-render
  };

  return {
    token,
    isAuthenticated: !!token,
    setToken,
    clearToken,
  };
}
