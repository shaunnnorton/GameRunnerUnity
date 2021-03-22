from api_app import app, db


class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(50),nullable=False)
    progress = db.Column(db.Integer)
    games = db.relationship('Game',secondary="user_game",back_populates='users')
    password = db.Column(db.String(80),nullable=False)


class Game(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    title = db.Column(db.String(80))
    box_art = db.Column(db.String(200))
    users = db.relationship('User',secondary="user_game", back_populates="games")
    def __str__(self):
        return f'<Game: {self.title}>'
    def __repr__(self):
        return f'<Game: {self.title}>'

user_game_table = db.Table('user_game',
    db.Column('user_id',db.Integer,db.ForeignKey('user.id')),
    db.Column('game_id',db.Integer,db.ForeignKey('game.id'))

)