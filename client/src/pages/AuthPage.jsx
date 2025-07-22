import React, { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext.jsx';
import { useNavigate } from 'react-router-dom';

const AuthPage = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');
  const { login, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated) {
      navigate('/home');
    }
  }, [isAuthenticated, navigate]);

  useEffect(() => {
    // Load Google Sign-In script
    const script = document.createElement('script');
    script.src = 'https://accounts.google.com/gsi/client';
    script.async = true;
    script.defer = true;
    document.body.appendChild(script);

    script.onload = () => {
      if (window.google) {
        window.google.accounts.id.initialize({
          client_id: '107422825913-udb7nt8k5k9k742ii0ah20bmmgmce833.apps.googleusercontent.com',
          callback: handleGoogleResponse,
        });

        window.google.accounts.id.renderButton(
          document.getElementById('google-signin-button'),
          {
            theme: 'outline',
            size: 'large',
            width: 300,
            text: 'signin_with'
          }
        );
      }
    };

    return () => {
      document.body.removeChild(script);
    };
  }, []);

  const handleGoogleResponse = async (response) => {
    setIsLoading(true);
    setError('');

    try {
      const authResult = await login(response.credential);
      
      if (authResult.isNewUser) {
        // Show welcome message for new users
        console.log('Welcome new user!');
      }
      
      navigate('/home');
    } catch (err) {
      setError(err.message || 'Failed to sign in with Google');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center p-4">
      <div className="card w-full max-w-md p-8">
        <div className="text-center mb-8">
          <div className="w-16 h-16 bg-primary-600 rounded-full flex items-center justify-center mx-auto mb-4">
            <svg className="w-8 h-8 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
            </svg>
          </div>
          <h1 className="text-2xl font-bold text-gray-900 mb-2">Password Manager</h1>
          <p className="text-gray-600">Secure your passwords with ease</p>
        </div>

        {error && (
          <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg mb-6">
            {error}
          </div>
        )}

        <div className="space-y-6">
          <div className="text-center">
            <h2 className="text-lg font-semibold text-gray-900 mb-4">
              Sign in to your account
            </h2>
            <p className="text-sm text-gray-600 mb-6">
              Use your Google account to access your password vault
            </p>
            
            <div className="flex justify-center">
              {isLoading ? (
                <div className="flex items-center justify-center py-3 px-6">
                  <div className="animate-spin rounded-full h-6 w-6 border-b-2 border-primary-600"></div>
                  <span className="ml-2 text-gray-600">Signing in...</span>
                </div>
              ) : (
                <div id="google-signin-button"></div>
              )}
            </div>
          </div>

          <div className="border-t border-gray-200 pt-6">
            <div className="text-center text-sm text-gray-500">
              <p>Your passwords are encrypted and secure.</p>
              <p className="mt-1">We never store your Google credentials.</p>
            </div>
          </div>
        </div>

        <div className="mt-8 text-center">
          <div className="flex items-center justify-center space-x-4 text-xs text-gray-400">
            <span>🔒 End-to-end encrypted</span>
            <span>•</span>
            <span>🛡️ Zero-knowledge</span>
            <span>•</span>
            <span>🔐 Secure</span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AuthPage;