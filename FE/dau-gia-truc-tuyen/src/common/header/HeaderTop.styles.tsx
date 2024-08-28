import { styled } from '@mui/material';

const headerContainerStyles = {
  height: '4rem',
  width: '100%',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'space-between',
  padding: '0 1rem',
  backgroundColor: '#1a202c',
};

const titleStyles = {
  fontSize: '1.25rem',
  fontWeight: 'bold',
  color: '#f7fafc',
};

const userInfoStyles = {
  marginRight: '0.5rem',
  border: '1px solid white',
  borderRadius: '4px',
  padding: '0.5rem 1rem',
  color: '#f7fafc',
};

// Styled components
export const HeaderContainer = styled('div')(headerContainerStyles);

export const Title = styled('div')(titleStyles);

export const UserInfo = styled('span')(userInfoStyles);
