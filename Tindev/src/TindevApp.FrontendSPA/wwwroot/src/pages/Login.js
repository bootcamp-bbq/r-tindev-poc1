import React, { useState } from 'react';
import './Login.css';

import api from '../services/api';
import tokenApi from '../services/tokenApi';

import logo from '../assets/logo.svg';

export default function Login({ history }) {
  const [usernameText, setUsernameText] = useState('');

  async function handleSubmit(e) {
    e.preventDefault();

    const body = `username=${usernameText}`;
    const headers = {
      'Content-Type': 'application/x-www-form-urlencoded'
    }
    const response = await api.post('/users/authenticate', body, headers)

    const { token, username } = response.data;

    tokenApi.save(token);

    history.push(`/dev/${username}`);
  }

  return (
    <div className="login-container">
      <form onSubmit={handleSubmit}>
        <img src={logo} alt="Tindev"/>
        <input 
          placeholder="Digite seu usuário no Github"
          value={usernameText}
          onChange={e => setUsernameText(e.target.value)}
        />
        <button type="submit">Enviar</button>
      </form>
    </div>
  );
}