import NextAuth, { Profile } from 'next-auth';
import { OIDCConfig } from 'next-auth/providers';
import DuendeIDS6Provider from 'next-auth/providers/duende-identity-server6';

export const { handlers, signIn, signOut, auth } = NextAuth({
  providers: [
    DuendeIDS6Provider({
      id: 'id-server',
      clientId: 'nextApp',
      clientSecret: 'secret',
      issuer: 'http://localhost:5001',
      authorization: {
        params: {
          scope: 'openid profile AuctionService scope2 scope1 offline_access',
        },
      },
      idToken: true,
    } as OIDCConfig<Omit<Profile, 'username'>>),
  ],
  callbacks: {
    async authorized({ auth }) {
      return !!auth;
    },
    async jwt({ token, profile, account }) {
      if (account && account.access_token) {
        token.accessToken = account.access_token;
      }
      if (profile) {
        token.username =
          profile.username ||
          profile.preferred_username ||
          profile.name ||
          'Unknown User';
      }
      return token;
    },
    async session({ session, token }) {
      if (token) {
        session.user.username = token.username || 'Unknown User';
        session.accessToken = token.accessToken;
      }
      return session;
    },
    redirect({ url, baseUrl }) {
      // Allows relative callback URLs
      if (url.startsWith('/')) return `${baseUrl}${url}`;
      // Allows callback URLs on the same origin
      else if (new URL(url).origin === baseUrl) return url;
      return baseUrl;
    },
  },
});
