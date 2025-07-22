import React, { createContext, useContext, useState, useEffect } from 'react';
import { authService } from '../services/authService.js';

const AuthContext = createContext();

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = authService.getToken();
    const savedUser = authService.getCurrentUser();
    
    if (token && savedUser) {
      setUser(savedUser);
    }
    setLoading(false);
  }, []);

  const login = async (idToken) => {
    try {
      const authResponse = await authService.googleAuth(idToken);
      
      localStorage.setItem('token', authResponse.token);
      localStorage.setItem('user', JSON.stringify({
        userId: authResponse.userId,
        name: authResponse.name,
        email: authResponse.email,
        profilePictureUrl: authResponse.profilePictureUrl,
        isNewUser: authResponse.isNewUser
      }));
      
      setUser({
        userId: authResponse.userId,
        name: authResponse.name,
        email: authResponse.email,
        profilePictureUrl: authResponse.profilePictureUrl,
        isNewUser: authResponse.isNewUser
      });
      
      return authResponse;
    } catch (error) {
      throw error;
    }
  };

  const logout = () => {
    authService.logout();
    setUser(null);
  };

  const value = {
    user,
    login,
    logout,
    loading,
    isAuthenticated: !!user
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};