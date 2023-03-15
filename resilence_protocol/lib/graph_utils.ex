defmodule GraphUtils do
  @moduledoc """
  utility functions to work with the graph
  """
  @priv_dir "#{:code.priv_dir(:resilence_protocol)}"

  @lib_dir @priv_dir <> "/lib"
  @graphs_dir @lib_dir <> "/graphs/"
  # will not work in windows
  @dot_binary "/usr/bin/dot"
  @graph_images_dir @lib_dir <> "/graphs/images/"

  @doc """
  given the graph, converts it to a png image for
  visualization.
  """
  @spec graph_to_image(Graph.t(), String.t()) :: :ok
  def graph_to_image(graph, file_name \\ "temp_graph") do
    path = @graphs_dir <> file_name
    graph_dot_file_path = to_dot_file(graph, path)
    {"", 0} = dot_to_png(graph_dot_file_path)
    :ok
  end

  @spec to_dot_file(Graph.t()) :: String.t()
  def to_dot_file(graph, filename \\ "temp") do
    filename = filename <> ".dot"

    {:ok, dot_info} = Graph.to_dot(graph)

    :ok = File.write!(filename, dot_info)
    filename
  end

  @spec dot_to_png(atom()) :: {Collectable.t(), non_neg_integer}
  def dot_to_png(binary \\ :dot, file_path) do
    binary =
      case binary do
        :dot -> @dot_binary
      end

    png_file = @graph_images_dir <> Path.basename(file_path, ".dot") <> ".png"

    System.cmd(binary, ["-T", "png", file_path, "-o", png_file])
  end
end
