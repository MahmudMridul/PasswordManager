import React from "react";
import {
	BrowserRouter as Router,
	Routes,
	Route,
	Navigate,
} from "react-router-dom";
import { AuthProvider } from "./context/AuthContext.jsx";
import ProtectedRoute from "./components/ProtectedRoute.jsx";
import AuthPage from "./pages/AuthPage.jsx";
import HomePage from "./pages/HomePage.jsx";

export default function App() {
	return (
		<AuthProvider>
			<Router>
				<div className="App">
					<Routes>
						<Route path="/auth" element={<AuthPage />} />
						<Route
							path="/home"
							element={
								<ProtectedRoute>
									<HomePage />
								</ProtectedRoute>
							}
						/>
						<Route path="/" element={<Navigate to="/auth" replace />} />
					</Routes>
				</div>
			</Router>
		</AuthProvider>
	);
}
