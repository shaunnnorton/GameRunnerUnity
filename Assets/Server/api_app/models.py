from api_app import app, db


class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    progress = db.Column(db.Integer)
    games = db.relationship('Game',secondary="user_game",back_populates='users')


class Game(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    title = db.Column(db.String(80))
    box_art = db.Column(db.String(200))
    users = db.relationship('User',secondary="user_game", back_populates="games")


user_game_table = db.Table('user_game',
    db.Column('user_id',db.Integer,db.ForeignKey('user.id')),
    db.Column('game_id',db.Integer,db.ForeignKey('game.id'))

)