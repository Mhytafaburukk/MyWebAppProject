import React, { useState, useEffect } from 'react';

const Dashboard = () => {
    const [data, setData] = useState([]);

    useEffect(() => {
        fetch('https://localhost:5001/weatherforecast')
            .then(response => response.json())
            .then(data => setData(data));
    }, []);

    return (
        <div className="dashboard">
            <h2>Weather Dashboard</h2>
            <div className="weather-grid">
                {data.map((item, index) => (
                    <div key={index} className="weather-card">
                        <h3>{item.date}</h3>
                        <p>{item.temperatureC}°C</p>
                        <p>{item.summary}</p>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default Dashboard;