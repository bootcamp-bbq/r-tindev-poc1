import React, { useEffect, useState } from 'react';
import io from 'socket.io-client';
import { Link } from 'react-router-dom';
import './Main.css';

import api, { headerFactory } from '../services/api';

import logo from '../assets/logo.svg';
import dislike from '../assets/dislike.svg';
import like from '../assets/like.svg';
import itsamatch from '../assets/itsamatch.png';

import tokenApi from '../services/tokenApi';

export default function Main({ match }) {
  const [users, setUsers] = useState([]);
  const [matchDev, setMatchDev] = useState(null);

  useEffect(() => {
    async function loadUsers() {
      const headers = headerFactory(tokenApi.load());
      const response = await api.get(`/devs`, headers);

      setUsers(response.data.items);
    }

    loadUsers();
  }, [match.params.id]);

  //useEffect(() => {
  //   const socket = io('http://localhost:3333', {
  //     query: { user: match.params.id }
  //   });

  //   socket.on('match', dev => {
  //     setMatchDev(dev);
  //   })
  // }, [match.params.id]);

  async function handleLike(username, id) {
    await api.post(`/devs/${username}/like/add`, {}, headerFactory(tokenApi.load()));

    setUsers(users.filter(user => user.id !== id));
  }

  async function handleDislike(id) {
    await api.post(`/devs/${id}/dislikes`, null, {
      headers: { user: match.params.id },
    })

    setUsers(users.filter(user => user._id !== id));
  }

  return (
    <div className="main-container">
      <Link to="/">
        <img src={logo} alt="Tindev" />
      </Link>

      { users.length > 0 ? (
        <ul>  
          {users.map(user => (
            <li key={user.id}>
              <img src={user.avatar} alt={user.name} />
              <footer>
                <a href={user.githubUri} target="_blank">
                  <strong>{user.name}</strong> <span>{user.user}</span>
                </a>
                <p>{user.bio}</p>
              </footer>

              <div className="buttons">
                <button type="button" onClick={() => handleDislike(user._id)}>
                  <img src={dislike} alt="Dislike" />
                </button>
                <button type="button" onClick={() => handleLike(user.name, user.id)}>
                  <img src={like} alt="Like" />
                </button>
              </div>
            </li>
          ))}
        </ul>
      ) : (
        <div className="empty">Acabou :(</div>
      ) }

      { matchDev && (
        <div className="match-container">
          <img src={itsamatch} alt="It's a match" />

          <img className="avatar" src={matchDev.avatar} alt=""/>
          <strong>{matchDev.name}</strong>
          <p>{matchDev.bio}</p>

          <button type="button" onClick={() => setMatchDev(null)}>FECHAR</button>
        </div>
      ) }
    </div>
  )
}