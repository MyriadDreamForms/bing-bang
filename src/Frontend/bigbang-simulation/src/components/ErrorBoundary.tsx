import React from 'react';

interface ErrorBoundaryState {
  hasError: boolean;
  error?: Error;
}

interface ErrorBoundaryProps {
  children: React.ReactNode;
}

class ErrorBoundary extends React.Component<ErrorBoundaryProps, ErrorBoundaryState> {
  constructor(props: ErrorBoundaryProps) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError(error: Error): ErrorBoundaryState {
    return { hasError: true, error };
  }

  componentDidCatch(error: Error, errorInfo: React.ErrorInfo) {
    console.error('Error Boundary yakaladı:', error, errorInfo);
  }

  render() {
    if (this.state.hasError) {
      return (
        <div style={{
          padding: '2rem',
          textAlign: 'center',
          background: 'var(--card-bg)',
          border: '1px solid var(--error-color)',
          borderRadius: '8px',
          margin: '2rem'
        }}>
          <h2 style={{ color: 'var(--error-color)' }}>⚠️ Bir Hata Oluştu</h2>
          <p>Uygulama beklenmedik bir hatayla karşılaştı.</p>
          <details style={{ marginTop: '1rem', textAlign: 'left' }}>
            <summary>Hata Detayları</summary>
            <pre style={{ 
              background: '#1a1a1a', 
              padding: '1rem', 
              borderRadius: '4px',
              overflow: 'auto',
              fontSize: '0.8rem'
            }}>
              {this.state.error?.stack}
            </pre>
          </details>
          <button 
            onClick={() => this.setState({ hasError: false, error: undefined })}
            style={{
              padding: '0.5rem 1rem',
              background: 'var(--primary-color)',
              color: 'white',
              border: 'none',
              borderRadius: '4px',
              cursor: 'pointer',
              marginTop: '1rem'
            }}
          >
            🔄 Yeniden Dene
          </button>
        </div>
      );
    }

    return this.props.children;
  }
}

export default ErrorBoundary;
