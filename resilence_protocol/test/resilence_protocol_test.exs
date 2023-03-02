defmodule ResilenceProtocolTest do
  use ExUnit.Case
  doctest ResilenceProtocol

  test "greets the world" do
    assert ResilenceProtocol.hello() == :world
  end
end
