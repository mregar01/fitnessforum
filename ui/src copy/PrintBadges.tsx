import React from 'react';
import PrintDate from './PrintDate';
import './App.css'
import bronzetrophy from './images/bronzetrophy.jpeg'
import silvertrophy from './images/silvertrophy.jpeg'
import goldtrophy from './images/goldtrophy.avif'

interface Badge {
  id: number;
  name: string;
  date: string;
}

interface PrintBadgesProps {
  badges: Badge[] | undefined;
  section: 'bronze' | 'silver' | 'gold';
}

const PrintBadges: React.FC<PrintBadgesProps> = ({ badges, section }) => {
  const getBadgeImage = () => {
    switch (section) {
      case 'bronze':
        return bronzetrophy;
      case 'silver':
        return silvertrophy;
      case 'gold':
        return goldtrophy;
      default:
        return '';
    }
  };

  const getDotClassName = () => {
    switch (section) {
      case 'bronze':
        return 'bronze-dot-badge';
      case 'silver':
        return 'silver-dot-badge';
      case 'gold':
        return 'gold-dot-badge';
      default:
        return '';
    }
  };

  if (!badges) {
    return null;
  }

  return (
    <div className={`col-sm-12 col-lg-4 ${section} border border-secondary rounded my-2` }>
        <div className='row'>
          <div className='col-2 col-md-2 col-lg-3 col-xl-2'>
            <img className='trophy' src={getBadgeImage()} alt={`${section}-trophy`} />
          </div>
          <div className='col'>
            <strong>{badges.length}</strong>
            <br />
            <small>{`${section} badges`}</small>
          </div>
        </div>
        <div className='row my-4 d-flex'>
          {badges.length > 0 ? (
           badges.slice(0, 5).map((badge) => (
              <div className='row' key={badge.id}>
                <div className='d-flex justify-content-between w-100'>
                  <div className='bg-dark text-white rounded my-1 px-1'>
                    <span className={getDotClassName()}></span>
                    <span className='small-text'>{badge.name}</span>
                  </div>
                  <div className='ml-auto'>
                    <small className='small-text'>
                      <PrintDate dateString={badge.date} monthType='short' type='user' />
                    </small>
                  </div>
                </div>               
              </div>
            ))
          ) : (
            <p></p>
          )}
        </div>      
    </div>
  );
};

export default PrintBadges;

